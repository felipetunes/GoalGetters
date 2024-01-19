using GoalGetters.Models;
using GoalGetters.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;


public class ApiService<T> : IApiService<T>
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<T> GetById(int id)
    {
        string url = $"http://localhost:8080/api/v1/{typeof(T).Name.ToLower()}/getbyid/{id}";
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }
        else
        {
            // Handle the error
            throw new Exception($"Error retrieving {typeof(T).Name.ToLower()}");
        }
    }

    public async Task<IEnumerable<T>> GetByName(string name)
    {
        string url = $"http://localhost:8080/api/v1/{typeof(T).Name.ToLower()}/getbyname/{Uri.EscapeDataString(name)}";
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<IEnumerable<T>>();
            return data;
        }
        else
        {
            // Handle the error
            throw new Exception($"Error retrieving {typeof(T).Name.ToLower()}");
        }
    }

    public async Task<T> UpdatePlayer(int id, string name, int idteam, string city, string country, DateTime birth, string height)
    {
        // Verifique se os argumentos são válidos
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(height))
        {
            throw new ArgumentException("Name, city, country, and height must not be null or empty.");
        }

        // Constrói a URL
        string url = $"http://localhost:8080/api/v1/player/update?id={id}&name={name}&idteam={idteam}&city={city}&country={country}&birth={birth.ToString("yyyy-MM-dd")}&height={height}";

        // Envia a solicitação PUT
        HttpResponseMessage response = await _client.PutAsync(url, null);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsAsync<T>();
            return result;
        }
        else
        {
            // Handle the error
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error updating player: {response.StatusCode} - {error}");
        }
    }
}

