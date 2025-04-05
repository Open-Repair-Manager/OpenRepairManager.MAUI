using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;
using OpenRepairManager.Common.Models;
using OpenRepairManager.Common.Models.ApiModels;

namespace OpenRepairManager.MAUI.Services;

public static class ApiService
{
   private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    private static HttpClient _client = new HttpClient()
    {
        BaseAddress = new Uri(Preferences.Default.Get("ApiUrl", "")),
        DefaultRequestHeaders =
        {
            
            {"ApiKey", Preferences.Default.Get("ApiKey", "")}
        }
    };


    public static async Task<RepairItem> GetRepairItemAsync(int id)
    {
        try
        {
            return await _client.GetFromJsonAsync<RepairItem>($"Item/{id}");
        }
        catch
        {
            return new RepairItem();
        }
    }
    
    public static async Task<IList<Session>> GetSessionsAsync(int count, string orderBy)
    {
        try
        {
            return await _client.GetFromJsonAsync<IList<Session>>($"Sessions?count={count}&orderBy={orderBy}");
        }
        catch
        {
            return new List<Session>();
        }
    }

    public static async Task<IList<RepairItem>> GetStatsAsync(string slug, string categorySlug)
    {
        if (categorySlug == "Reception")
            return await _client.GetFromJsonAsync<IList<RepairItem>>($"Session/{slug}");
        else
            return await _client.GetFromJsonAsync<IList<RepairItem>>($"Session/{slug}/{categorySlug}");
    }
    
    public static async Task<Session> GetSessionAsync(string sessionSlug)
    {
        var exists = _client.GetAsync($"Session/View/{sessionSlug}").Result.StatusCode;
        if (exists == HttpStatusCode.NotFound)
        {
            return new Session()
            {
                SessionName = "error"
            };
        }
        Session session = await _client.GetFromJsonAsync<Session>($"Session/View/{sessionSlug}");
        if (session == null)
        {
            return new Session()
            {
                SessionName = "error"
            };
        }
        return session;
    }

    public static async Task<Response> UpdateItemAsync(RepairItem item)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new Response()
            {
                Message = "No internet connection",
                Status = "Fail"
            };
        
        item.ProductAge = DateTime.Now.Year - item.ProductYear;
        string json = JsonSerializer.Serialize<RepairItem>(item, _serializerOptions);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        Debug.WriteLine(json);
        var responseMessage = await _client.PutAsync($"/api/RepairItem/Edit/{item.ID}", content);
        Debug.WriteLine(responseMessage.StatusCode);
        Debug.WriteLine(item);
        return responseMessage.Content.ReadFromJsonAsync<Response>().Result;
    }
    
    public static async Task<Response> NewItemAsync(RepairItem item)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new Response()
            {
                Message = "No internet connection",
                Status = "Fail"
            };

        try
        {
            item.ProductAge = DateTime.Now.Year - item.ProductYear;
            string json = JsonSerializer.Serialize<RepairItem>(item, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(json);
            var responseMessage = await _client.PostAsync("/api/RepairItem/Add", content);
            Debug.WriteLine(responseMessage.StatusCode);
            Debug.WriteLine(item);
            return responseMessage.Content.ReadFromJsonAsync<Response>().Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Debug.WriteLine(e.Message);
            return new Response()
            {
                Message = e.Message,
                Status = "Fail"
            };
        }
        
    }

    public static async Task<bool> AreSettingsValidAsync(string apiKey, string apiUrl)
    {
        Debug.WriteLine(apiUrl);
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return false;

        try
        {
            
            HttpClient testClient = new HttpClient()
            {
                BaseAddress = new Uri(apiUrl),
                DefaultRequestHeaders =
                {
                    //test API Key - localhost db only - Store in environment variable later
                    { "ApiKey", apiKey }
                }
            };
            try
            {
                var response = await testClient.GetAsync($"Sessions?count=0&orderBy=asc");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

}