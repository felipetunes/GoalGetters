using GoalGetters.Models;
using GoalGetters.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using X.PagedList;

public class ApiService<T> : IApiService<T>
{
    private readonly HttpClient _client;
    const string urlApi = "http://localhost:8080/api/v1/";

    public ApiService(HttpClient client)
    {
        _client = client;
    }

    async public Task<HttpResponseMessage> Delete(int id, string entity)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{urlApi}{entity}/delete/{id}");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error retrieving {typeof(T).Name.ToLower()}");
            }

            return response;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<T> GetById(int id)
    {
        string url = $"{urlApi}{typeof(T).Name.ToLower()}/getbyid/{id}";
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }
        else
        {
            // Return default value of T
            return default(T);
        }
    }

    public async Task<List<T>> GetByName(string name)
    {
        string url = $"{urlApi}{typeof(T).Name.ToLower()}/getbyname/{Uri.EscapeDataString(name)}";
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<List<T>>();
            return data;
        }
        else
        {
            // Handle the error
            throw new Exception($"Error retrieving {typeof(T).Name.ToLower()}");
        }
    }

    public async Task<List<Player>> GetPlayersByTeamId(int teamId)
    {
        var response = await _client.GetAsync($"{urlApi}player/getbyidteam/{teamId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Erro ao obter jogadores: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var players = JsonConvert.DeserializeObject<List<Player>>(json);
        return players;
    }

    public async Task<string> Insert<T>(T obj)
    {
        string url = $"{urlApi}{typeof(T).Name.ToLower()}/insert";
        HttpResponseMessage response = await _client.PostAsJsonAsync(url, obj);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle the error
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error updating entity: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T> Update<T>(int id, string name, string city, string country, int? idteam = null, DateTime? birth = null, string height = null, string position = null)
    {
        // Verifique se os argumentos são válidos
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
        {
            throw new ArgumentException("Name, city and country must not be null or empty.");
        }

        // Constrói a URL
        string url = $"{urlApi}{typeof(T).Name.ToLower()}/update?id={id}&name={name}&city={city}&country={country}";

        if (idteam.HasValue)
        {
            url += $"&idteam={idteam.Value}";
        }

        if (birth.HasValue)
        {
            url += $"&birth={birth.Value.ToString("dd/MM/yyyy")}";
        }

        if (!string.IsNullOrEmpty(height))
        {
            url += $"&height={height}";
        }

        if (!string.IsNullOrEmpty(position))
        {
            url += $"&position={position}";
        }

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
            throw new HttpRequestException($"Error updating entity: {response.StatusCode} - {error}");
        }
    }

    public async Task<List<T>> GetAll<T>(string endpoint)
    {
        string url = $"{urlApi}{typeof(T).Name.ToLower()}/{endpoint}";
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<List<T>>();
            return data;
        }
        else
        {
            // Return default value of List<T>
            return default(List<T>);
        }
    }

}

