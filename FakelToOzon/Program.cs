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
        Console.WriteLine("При проблемах с кодировкой ставь <Кириллица (ISO)> или <Windows-1251>");
        using (Process process = new Process())
        {
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //startInfo.FileName = @"..\data\VPN\OpenVPN\bin\openvpn.exe";
            //startInfo.Arguments = @"--cd ""../data/VPN/OpenVPN/config/"" --config UserVPN.ovpn --verb 11";
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    // This code will only be executed in Debug mode

            //    startInfo.FileName = @"..\..\..\data\VPN\OpenVPN\bin\openvpn.exe";
            //    startInfo.Arguments = @"--cd ""../../../data/VPN/OpenVPN/config/"" --config UserVPN.ovpn --verb 11";
            //}
            //startInfo.Verb = "runas";
            //process.StartInfo = startInfo;
            //process.Start();
            //Console.WriteLine("Подключен Впн, не забудь его выключить!");
            //Thread.Sleep(5000);

            GlobalVariables globalVariables = new();
            string _baseUrl = globalVariables.BaseURL;
            CreateExcelTable createExcelTable = new CreateExcelTable();
            JsonToOzon jsonToOzon = new JsonToOzon();
            ConvertToJson convertToJson = new ConvertToJson();

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


