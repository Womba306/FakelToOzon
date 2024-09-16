using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class OzonApiClient
{
    private readonly HttpClient _httpClient;

    public OzonApiClient(string baseUrl, string apiKey, string clientId)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);
    }

    public async Task<string> GetAsync(string path)
    {
        try
        {
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    public async Task<string> PostAsync(string path, HttpContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync(path, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    public async Task<string> PutAsync(string path, HttpContent content)
    {
        try
        {
            var response = await _httpClient.PutAsync(path, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }

    public async Task<string> DeleteAsync(string path)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            throw;
        }
    }
}