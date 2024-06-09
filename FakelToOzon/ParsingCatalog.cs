using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakelToOzon
{
    public class ParsingCatalog
    {
        public IEnumerable<object> ParsingCatalogBlock(string url)
        {
            try
            {
                using (HttpClientHandler _handler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.All })
                {
                    using (var _client = new HttpClient(_handler))
                    {
                        
                        using (HttpResponseMessage _response = _client.GetAsync(url).Result)
                        {
                            if (_response.IsSuccessStatusCode)
                            {
                                var html = _response.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument _document = new HtmlAgilityPack.HtmlDocument();
                                    _document.LoadHtml(html);
                                    var catalogBlock = _document.DocumentNode.SelectNodes(".//div[@class='categories__list']//a[@class='categories__item lazyload']");
                                    if (catalogBlock != null && catalogBlock.Count > 0)
                                    {
                                        foreach (var catalog in catalogBlock)
                                        {
                                            yield return catalog.GetAttributeValue("href", "");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Список пуст, жди обновлений");
                                        yield return null;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Пустой HTML");
                                    yield return null;
                                }
                            }
                            else
                            {
                                yield return null;
                            }
                        }
                    }
                }

            }
            finally
            {
                Console.WriteLine("Блок Каталоги найдены");
            }

        }
    }
}
