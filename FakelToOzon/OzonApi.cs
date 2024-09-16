using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FakelToOzon
{
    public class OzonApi
    {
        private class JsSeralise
        {
            [JsonPropertyName("offer_id")]
            string Articule {  get; set; }
            [JsonPropertyName("stock")]
            int Stocs { get; set; }
            [JsonPropertyName("warehouse_id")]
            int WarehouseID { get; set; }
        }
        JsonToOzon jsonToOzon = new JsonToOzon();
        public async Task UpdateStocksAsync()
        {
            JsonToOzon jsonToOzon = new JsonToOzon();

            var ozonApiClient = new OzonApiClient("https://api-seller.ozon.ru", "dd895049-b2b9-4945-a397-72c39b0e109c", "1391788");

            // Пример POST запроса
            var content = new StringContent("{\"key\":\"value\"}", System.Text.Encoding.UTF8, "application/json");
            response = await ozonApiClient.PostAsync("/v2/order/create", content);
            Console.WriteLine(response);
        }
        }



      
        
        
    }

}
