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
        //Включить перед началом работы

        Console.WriteLine("При проблемах с кодировкой ставь <Кириллица (ISO)> или <Windows-1251>");
        using (Process process = new Process())
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = @"C:\Program Files\OpenVPN\bin\openvpn.exe";
            startInfo.Arguments = @"--cd ""C:\Program Files\OpenVPN\config"" --config UserVPN.ovpn --verb 11";
            startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine("Подключен Впн, не забудь его выключить");
            Thread.Sleep(5000);


            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;

            JsonToOzon jsonToOzon = new JsonToOzon();
            ConvertToJson convertToJson = new ConvertToJson();
            foreach (var item in jsonToOzon.CreateJson())
            {
                convertToJson.ConvertToJsonDev(item);
            }
            process.Close();
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


