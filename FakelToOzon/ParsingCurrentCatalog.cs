using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FakelToOzon
{
    public class ParsingCurrentCatalog 
    {
        public IEnumerable<object> ParsingCurrentCatalogBlock() 
        {
            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;

            ParsingCatalog catalogBlock = new ParsingCatalog();

            foreach (string catalogsUrls in catalogBlock.ParsingCatalogBlock(url: $"{_baseUrl}/catalog/"))
            {
                int countpages = 1;
                int i = 1;
                do
                {

                    try
                    {
                        using (HttpClientHandler _handler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.All })
                        {
                            using (var _client = new HttpClient(_handler))
                            {
                                string url = $"{_baseUrl + catalogsUrls.ToString()}?PAGEN_1={i}";
                                using (HttpResponseMessage _response = _client.GetAsync(url).Result)
                                {
                                    if (_response.IsSuccessStatusCode)
                                    {
                                        var html = _response.Content.ReadAsStringAsync().Result;
                                        if (!string.IsNullOrEmpty(html))
                                        {
                                            HtmlAgilityPack.HtmlDocument _document = new HtmlAgilityPack.HtmlDocument();
                                            _document.LoadHtml(html);
                                            var page = _document.DocumentNode.SelectNodes(".//div[@class='catalog__pagination']//a[@class='pagination__item']");
                                            List<int> pageindex = new List<int>();
                                            foreach (var currentItem in page)
                                            {
                                                if (int.TryParse(currentItem.InnerText.ToString(), out var num))
                                                {
                                                    pageindex.Add(num);
                                                    
                                                }
                                            }
                                            countpages = pageindex.Max();



                                            var item = _document.DocumentNode.SelectNodes(".//div[@class='catalog__product-item product']//a[@class='product__image-wrapper']");
                                            if (item != null && item.Count > 0)
                                            {
                                                foreach (var currentItem in item)
                                                {
                                                    Console.WriteLine(currentItem.GetAttributeValue("href", ""));
                                                    yield return currentItem.GetAttributeValue("href", "");
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
                        Console.WriteLine("Блок отработал");
                    }
                i++;
                } while (i< countpages);

            }


        }
           
    }
}
