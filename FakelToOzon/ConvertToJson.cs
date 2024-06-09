using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using Medallion;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace FakelToOzon
{
    public class ConvertToJson() {
        public string ConvertToJsonDev(JSONBuilder jsonBuilder)
        {
            string jsonString = JsonConvert.SerializeObject(jsonBuilder, Formatting.Indented);
            Console.WriteLine(jsonString); // Вывод JSON-строки в консоль
            return jsonString;
        }

    }
}
