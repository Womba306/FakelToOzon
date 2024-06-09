using Medallion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FakelToOzon
{
    public class JSONBuilder()
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Price { get; set; }
        public string Articule { get; set; }
        public string CodeTNVED { get; set; }
        public string Weight { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public List<string> Size { get; set; }
        public List<string> Imgs { get; set; }
        public string Hash { get; set; }
        public string TRTS { get; set; }
        public string GOST { get; set; }
        public string Season { get; set; }
        public string Zipper { get; set; }
        public string Protects { get; set; }
        public string AllParameters { get; set; }
        
        //public string Zipper { get; set; }
        //public string Zipper { get; set; }



    }
    public class ParsingCurrentItem
    {
        private JSONBuilder builder = new JSONBuilder();
        public IEnumerable<HtmlAgilityPack.HtmlDocument>  ParsingCurrentItemData()
        {



            //Глобалы
            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;

            // Подключили говно
            ParsingCurrentCatalog parsingCurrentCatalog = new ParsingCurrentCatalog();
            ParsingBase parsingBase = new ParsingBase();

            foreach (string currentItemUrl in parsingCurrentCatalog.ParsingCurrentCatalogBlock())
            {
                if (currentItemUrl != null)
                {
                    var _document = parsingBase.GetHtmlDocument(currentItemUrl, _baseUrl);

                    
                    yield return _document;
                    
                   


                    


                }
                else
                {
                    Console.WriteLine("Ошибка в предмете");
                    yield return null;
                }
                
            }
            
        }
        //Список ссылок картинок
        public ParsingCurrentItem GetImgUrls(HtmlAgilityPack.HtmlDocument _document, string _baseUrl)
        {
            
            builder.Articule = _document.DocumentNode.SelectSingleNode(".//div[@class='item__wrapper wrapper']//div[@class='item__column']//div[@class='item__vendor']//span[@class='item__vendor-item']").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
            var imgUrls = _document.DocumentNode.SelectNodes(".//div[@class='swiper-wrapper']//a");
            List<string> images = new List<string>();
            if (imgUrls != null && imgUrls.Count > 0)
            {
                foreach (var currentUrl in imgUrls)
                {
                    //СЮДА JSON 
                    images.Add($"{_baseUrl}{currentUrl.GetAttributeValue("href", "")}");
                }
                builder.Imgs = images;


                 return this;
            }
            else
            {
                Console.WriteLine("Проблема в ссылках");
                return null;
            }
        }
        public ParsingCurrentItem GetSizesTablet(HtmlAgilityPack.HtmlDocument _document, string _baseUrl)
        {

            var tabletLine = _document.DocumentNode.SelectNodes(".//tbody[@class='product-table__product product-table__product_active']//tr[@class='product-table__size ']");
            if (tabletLine != null && tabletLine.Count > 0)
            {
                string[,] itemsList = new string[tabletLine.Count, 3];
                int cnt = 0;
                List<string> sizes = new List<string>();
                List<string> prices = new List<string>();
                foreach (var tabletItem in tabletLine)
                {
                    var currentItemData = tabletItem.SelectNodes(".//td");
                    if (currentItemData != null && currentItemData.Count >= 3)
                    {
                        itemsList[cnt, 0] = currentItemData[0].SelectSingleNode(".//div[@class='product-table__name']").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
                        itemsList[cnt, 1] = currentItemData[1].SelectSingleNode(".//span[@class='product-table__price product-table__price_disabled']").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
                        itemsList[cnt, 2] = currentItemData[2].SelectSingleNode(".//span[@class='product-table__available']").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
                        sizes.Add(itemsList[cnt, 0]);
                        prices.Add(itemsList[cnt, 1]);
                    }
                    cnt++;

                }
                builder.Size = sizes;
                builder.Price = prices;
                // size, price, product_count
               return this;


            }
            else
            {
                Console.WriteLine("Проблема в таблицах размерности");
                 return null;
            }

        }
        public ParsingCurrentItem GetDescription(HtmlAgilityPack.HtmlDocument _document, string _baseUrl)
        {
            builder.Description = _document.DocumentNode.SelectSingleNode(".//div[@class='info__descr']/div[@class='wysiwyg']/p").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
            //descroption
            return this;

        }
        public ParsingCurrentItem GetParametersTablet(HtmlAgilityPack.HtmlDocument _document, string _baseUrl)
        {

            var tabletLine = _document.DocumentNode.SelectNodes(".//div[@class='info__specs-wrapper']//table[@class='info__specs-table']//tr");
            if (tabletLine != null && tabletLine.Count > 0)
            {
                string[,] itemsList = new string[tabletLine.Count, 2];
                int cnt = 0;


                string codeTNVED = "";
                string trts = "";
                string weght = "";
                string gost = "";
                string season = "";
                string ziper = "";
                string brand = "";
                string[] protect;
                string data = null;
                string protectsOUT = "";
                foreach (var tabletItem in tabletLine)
                {
                    var currentItemData = tabletItem.SelectNodes(".//td");
                    if (currentItemData != null)
                    {
                        itemsList[cnt, 0] = tabletItem.SelectSingleNode(".//th").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
                        itemsList[cnt, 1] = currentItemData[0].InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');

                        switch (itemsList[cnt, 0])
                        {
                            case "Код ТН ВЭД:":
                                codeTNVED = itemsList[cnt, 1];

                                break;
                            case "ТР/ТС:":
                                trts = itemsList[cnt, 1];
                                break;
                            case "Вес (кг. за 1 шт.):":
                                weght = (Convert.ToDecimal(itemsList[cnt, 1].Replace(".",",")) * 1000).ToString();
                                break;
                            case "Марка/бренд:":
                                string logolink = currentItemData[0].SelectSingleNode(".//img[@class='info__logo lazyload']").GetAttributeValue("scr", "").ToString();
                                switch (logolink)
                                {
                                    case "/upload/uf/9da/Resurs-1.svg":
                                        itemsList[cnt, 1] = "Факел";
                                        break;
                                    case "/upload/uf/ac1/logo.svg":
                                        itemsList[cnt, 1] = "БарсПрофи";
                                        break;
                                    case "/upload/resize_cache/uf/f23/180_60_1/cropped_Logotip_Rabosiz_2022.png":
                                        itemsList[cnt, 1] = "Рабосиз";
                                        break;
                                    case "/upload/resize_cache/uf/3ad/180_60_1/FireShot-Capture-044-_-Press_tsentr-_ARTI_zavod_-_-arti_zavod.ru.png":
                                        itemsList[cnt, 1] = "АРТИ-Завод";
                                        break;
                                    case "/upload/uf/528/logo.svg":
                                        itemsList[cnt, 1] = "BENOVY";
                                        break;
                                    case "/upload/uf/812/8127d00e5969d98616d684e7782d6a3e.png":
                                        itemsList[cnt, 1] = "Скорпион";
                                        break;
                                    case "/upload/uf/245/Resurs-1.svg":
                                        itemsList[cnt, 1] = "Псков-Полимер";
                                        break;
                                    case "/upload/resize_cache/uf/18b/180_60_1/95097d7ccf5de167a38746e5f2daa547.jpg":
                                        itemsList[cnt, 1] = "Rutex";
                                        break;
                                    case "/upload/uf/6bb/logo.svg":
                                        itemsList[cnt, 1] = "Защитная линия";
                                        break;
                                    case "/upload/resize_cache/uf/54b/180_60_1/54bcd9b2ee60b3aa152ed9b0496ddc65.jpg":
                                        itemsList[cnt, 1] = "ЗападБалтОбувь";
                                        break;
                                    case "/upload/uf/e2a/ursus_top_logo.svg":
                                        itemsList[cnt, 1] = "URSUS";
                                        break;
                                    case "/upload/resize_cache/uf/bd7/180_60_1/bd7b8b0ce469e138793825044f1e0262.png":
                                        itemsList[cnt, 1] = "SURA";
                                        break;
                                    case "/upload/resize_cache/uf/6b2/180_60_1/portwest_logo_oj.png":
                                        itemsList[cnt, 1] = "Portwest";
                                        break;
                                    case "/upload/uf/e35/YAkhting.svg":
                                        itemsList[cnt, 1] = "Яхтинг";
                                        break;
                                    case "/upload/uf/ac6/logo.svg":
                                        itemsList[cnt, 1] = "ЭЛЕН";
                                        break;
                                    case "/upload/resize_cache/uf/6c2/180_60_1/6c2c5bb8d4041948ff7962ad0fb26641.png":
                                        itemsList[cnt, 1] = "ФЭСТ";
                                        break;
                                    case "/upload/resize_cache/uf/c83/180_60_1/c83b079a0189cc09ea5bf2f8c4abd4f4.png":
                                        itemsList[cnt, 1] = "ОРИОН-РТИ";
                                        break;
                                    case "/upload/uf/52e/52e5247b4abc6b48b75ddf43a181ca84.png":
                                        itemsList[cnt, 1] = "Лилия";
                                        break;
                                    case "/upload/uf/a01/Resurs-1.svg":
                                        itemsList[cnt, 1] = "Бриз-Кама";
                                        break;
                                    case "/upload/resize_cache/uf/866/180_60_1/866711a633d2edcb1ec07deed7075d0b.png":
                                        itemsList[cnt, 1] = "Блю лэйбл";
                                        break;
                                    case "/upload/resize_cache/uf/6f5/180_60_1/6f51acaeea54e6ea0282b813589c659f.png":
                                        itemsList[cnt, 1] = "Армакон";
                                        break;
                                    case "/upload/resize_cache/uf/846/180_60_1/8466e7c003794e83355a9a6bfc42e5c0.png":
                                        itemsList[cnt, 1] = "АМПАРО";
                                        break;
                                    case "/upload/resize_cache/uf/d8d/180_60_1/d8d8c2cd905c1b2ced42c09b2442de3b.png":
                                        itemsList[cnt, 1] = "VENTO";
                                        break;
                                    case "/upload/resize_cache/uf/f56/180_60_1/logo_black-_1_.png":
                                        itemsList[cnt, 1] = "SURGUT";
                                        break;
                                    case "/upload/uf/0a3/0a3483598708baae623571f83939aceb.png":
                                        itemsList[cnt, 1] = "Step";
                                        break;
                                    case "/upload/resize_cache/uf/c99/180_60_1/logo-_1_.png":
                                        itemsList[cnt, 1] = "STANDART";
                                        break;
                                    case "/upload/resize_cache/uf/a99/180_60_1/a99119570db1bfed37514e30710a44de.png":
                                        itemsList[cnt, 1] = "NordMan Extreme";
                                        break;
                                    case "/upload/resize_cache/uf/86c/180_60_1/ezgif_1_7fdef90553.gif":
                                        itemsList[cnt, 1] = "Med Fashion Lab";
                                        break;
                                    case "/upload/resize_cache/uf/e39/180_60_1/e39163847320cdb75d0c43902db41299.png":
                                        itemsList[cnt, 1] = "Manipula Specialist";
                                        break;
                                    case "/upload/resize_cache/uf/7a3/180_60_1/4oNsEGmmVqA-kopiya.jpg":
                                        itemsList[cnt, 1] = "Jeta Safety";
                                        break;
                                    case "/upload/uf/16d/logo.png":
                                        itemsList[cnt, 1] = "WPL";
                                        break;
                                    case "/upload/resize_cache/uf/dbe/180_60_1/ezgif_5_55ec707db7.gif":
                                        itemsList[cnt, 1] = "Chirton";
                                        break;
                                    case "/upload/uf/0ad/logo_alt.svg":
                                        itemsList[cnt, 1] = "Bolle";
                                        break;
                                    case "/upload/uf/854/picto47big.gif":
                                        itemsList[cnt, 1] = "ANSELL";
                                        break;
                                    default:
                                        itemsList[cnt, 1] = "Profline";
                                        break;
                                }
                                brand = itemsList[cnt, 1];
                                break;
                            case "ГОСТ:":
                                gost = itemsList[cnt, 1];
                                break;
                            case "Сезонность:":
                                season = itemsList[cnt, 1];
                                break;
                            case "Вид центральной застежки (куртка):":
                                ziper = itemsList[cnt, 1];
                                break;
                            case "Защитные свойства:":
                                var protects = currentItemData[0].SelectNodes(".//img[@class='info__shield-item']");
                                protect = new string[protects.Count];
                                int protect_cnt = 0;
                                
                                foreach (var item in protects)
                                { if (item.GetAttributeValue("data-tooltip", "").ToString() != null && item != null )
                                    {
                                        try
                                        {
                                            protect[cnt] = item.GetAttributeValue("data-tooltip", "").ToString();
                                        }
                                        catch { protect[cnt] = ""; }
                                    }
                                    else
                                    {
                                        protect[cnt] = "";
                                    }
                                    protectsOUT += protect[cnt].TrimStart(' ', '\n').TrimEnd(' ', '\n') + ".";
                                    protect_cnt++;

                                }
                                itemsList[cnt, 1] = protectsOUT;
                                break;



                        }

                    }
                    data += itemsList[cnt, 0] + itemsList[cnt, 1]+"\n";
                    cnt++;
                }
                builder.CodeTNVED = codeTNVED;
                builder.TRTS = trts;
                builder.Weight = weght;
                builder.GOST = gost;
                builder.Season = season;
                builder.Zipper = ziper;
                builder.Brand = brand;
                builder.Protects=protectsOUT;
                builder.AllParameters = data;

                Console.WriteLine("Параметры найдены");


                 return this;
            }
            else
            {
                Console.WriteLine("Проблема в таблицах характеристик");
                 return null;
            }
        }
        public ParsingCurrentItem GetNameAndColor(HtmlAgilityPack.HtmlDocument _document, string _baseUrl)
        {
            var block = _document.DocumentNode.SelectSingleNode(".//div[@class='content']/h1[@class='content__title wrapper']").InnerText.TrimStart(' ', '\n').TrimEnd(' ', '\n');
            string[] s = block.Split(",");
            string name = string.Join(",", s.Take(s.Length - 1));
            string color = s.Last();
            builder.Hash = GetHash(name).ToString();
            builder.Name = name;
            builder.Color = color;
             return this;

        }
        public ParsingCurrentItem GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            builder.Hash = Convert.ToBase64String(hash);

             return this ;
        }
        public JSONBuilder Build()
        {
            return builder;
        }



    }
    public class JsonToOzon
    {
        public IEnumerable< JSONBuilder >CreateJson()
        {
            //Глобалы
            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;
            
            var builder = new ParsingCurrentItem();
            foreach (var _document in builder.ParsingCurrentItemData())
            {
                builder.GetImgUrls(_document, _baseUrl);
                builder.GetSizesTablet(_document, _baseUrl);
                builder.GetDescription(_document, _baseUrl);
                builder.GetParametersTablet(_document, _baseUrl);
                builder.GetNameAndColor(_document, _baseUrl);
                Thread.Sleep(10000);


               yield return builder.Build();
            }
        } 
    }
    
    }
       
    

    



 

