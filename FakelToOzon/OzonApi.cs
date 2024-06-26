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
            
            // Open the new file
            var ozonApi = new OzonApi("https://api.ozon.ru", "your_client_id", "your_api_key");
            // Fill in the table with data from the Ozon API
            foreach (var builder in jsonToOzon.CreateJson())
            {
                JSONBuilder jSONBuilder = new JSONBuilder();
             

                await ozonApi.UpdateStockAsync(builder);
            }
        }



        private readonly string _apiUrl;
        private readonly string _clientId;
        private readonly string _apiKey;
        
        
        public OzonApi(string apiUrl, string clientId, string apiKey)
        {
            _apiUrl = apiUrl;
            _clientId = clientId;
            _apiKey = apiKey;
        }

        public async Task UploadProductAsync(JSONBuilder product)
        {
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", _clientId);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Api-Key", _apiKey);

            var response = await client.PostAsync(_apiUrl + "/v2/products/stocks", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to upload product: {response.StatusCode}");
            }
        }
        public async Task UpdateStockAsync(JSONBuilder product)
        {
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _clientId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Api-Key", _apiKey);

            var response = await client.PostAsync(_apiUrl + "/v2/products/stocks", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update stock: {response.StatusCode}");
            }
        }
        
        
    }

}
