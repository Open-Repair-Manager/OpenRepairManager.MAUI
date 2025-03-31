using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;
using OpenRepairManager.Common.Models;

namespace OpenRepairManager.MAUI.Services;

public static class ApiService
{
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

    public static async Task<bool> NewItemAsync(RepairItem item)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return false;

        try
        {
            await _client.PostAsJsonAsync("/api/RepairItem", item);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }

    public static async Task<bool> AreSettingsValidAsync(string apiKey, string apiUrl)
    {
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
            var response = await testClient.GetAsync($"Sessions?count=0&orderBy=asc");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

}