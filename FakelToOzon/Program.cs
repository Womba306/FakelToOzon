using FakelToOzon;
using Opapps.Lib.UserAgentsGenerator.Entities;
using Opapps.Lib.UserAgentsGenerator.Services;
using SoftEtherApi;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;




class Program
{
    static async Task Main()
    {
      

            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;
            CreateExcelTable createExcelTable = new CreateExcelTable();
            JsonToOzon jsonToOzon = new JsonToOzon();
            ConvertToJson convertToJson = new ConvertToJson();
            while (true)
            {
                createExcelTable.CreateTable();
            }
            
        
    }

       
}

// Оптимизировать ParsingCatalog, ParsingCurrentCatalog с ParsingBase

    //internal async Task<string> GetUserAgent()
    //{
    //    UAGenerator userAgentsGenerator = new UAGenerator();

    //    UserAgent userAgent = await userAgentsGenerator.GenerateAsync();
    //    return userAgent.Content;
    //}


