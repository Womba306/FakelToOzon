using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Opapps.Lib.UserAgentsGenerator.Services;
using Opapps.Lib.UserAgentsGenerator.Entities;


namespace FakelToOzon
{
    public class ParsingBase
    {
        //string head = GetUserAgent();
        public HtmlAgilityPack.HtmlDocument GetHtmlDocument(string _url, string _baseUrl)
        {
            

            try
            {
                
                using (HttpClientHandler _handler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.All })
                {
                    using (var _client = new HttpClient(_handler))
                    {


                        //_client.DefaultRequestHeaders(head);
                        string url = $"{_baseUrl + _url.ToString()}";
                        using (HttpResponseMessage _response = _client.GetAsync(url).Result)
                        {
                            
                            if (_response.IsSuccessStatusCode)
                            {
                                var html = _response.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(html))
                                {
                                    HtmlAgilityPack.HtmlDocument _document = new HtmlAgilityPack.HtmlDocument();
                                    _document.LoadHtml(html);
                                    return _document;

                                }
                                else
                                {
                                    Console.WriteLine("Список пуст, жди обновлений");
                                    return null;
                                }

                            }

                            else
                            {
                                Console.WriteLine("123");
                                return null;
                            }
                        }
                    }
                }

            }
            finally
            {
                Console.WriteLine("ParsingBase отработал");

            }
        }
    }
}

