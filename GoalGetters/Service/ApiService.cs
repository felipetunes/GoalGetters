using GoalGetters.Models;
using GoalGetters.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using X.PagedList;
using static GoalGetters.Helper.Enums;

public class ApiService<T> : IApiService<T> where T : class
{
    private readonly HttpClient _client;
    public static class ApiServiceConstants
    {
        public const string urlApi = "http://localhost:8080/api/v1/";
    }

    public ApiService(HttpClient client)
    {
        _client = client;
    }

    protected HttpClient GetClient()
    {
        return _client;
    }

    public async Task<HttpResponseMessage> Delete(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/delete/{id}");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error deleting {typeof(T).Name.ToLower()}");
            }

            return response;
        }
        catch (Exception ex)
        {
            // Handle the exception
            return null;
        }
    }

    public async Task<T> GetById(int id)
    {
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/getbyid/{id}";
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
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/getbyname/{Uri.EscapeDataString(name)}";
        HttpResponseMessage response = await _client.GetAsync(url);
        List<T> data = null; // Declare data here so it can be accessed later

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                data = JsonConvert.DeserializeObject<List<T>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deserialization: {ex.Message}");
            }
        }
        else
        {
            // Handle the error
            throw new Exception($"Error retrieving {typeof(T).Name.ToLower()}");
        }

        return data;
    }

    public async Task<List<T>> Register(string username, string password, byte[] photoBytes)
    {
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/register";

        // Cria um novo MultipartFormDataContent
        using var content = new MultipartFormDataContent();

        // Adiciona o nome de usuário e a senha ao conteúdo
        content.Add(new StringContent(username), "username");
        content.Add(new StringContent(password), "password");

        // Adiciona a foto ao conteúdo
        if (photoBytes != null)
        {
            content.Add(new ByteArrayContent(photoBytes), "photo", "photo.jpg");
        }

        HttpResponseMessage response = await _client.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsAsync<List<T>>();
            return data;
        }
        else
        {
            // Trata o erro
            throw new Exception($"Error creating {typeof(T).Name.ToLower()}");
        }
    }

    public async Task<string> Create(T obj)
    {
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/insert";
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

 

    public async Task<List<T>> GetAll(string endpoint)
    {
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/{endpoint}";
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

    public async Task<T> Update(T entity)
    {
        // Build the URL
        string url = $"{ApiServiceConstants.urlApi}{typeof(T).Name.ToLower()}/update";

        // Convert the entity to a JSON string
        var jsonContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");

        // Send the PUT request
        HttpResponseMessage response = await _client.PutAsync(url, jsonContent);
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

    internal async Task<Live> UpdateLive(Live match)
    {
        // Verifique se os argumentos são válidos
        if (match.HomeTeam == null || match.VisitingTeam == null || match.Championship == null || string.IsNullOrEmpty(match.Stadium) || string.IsNullOrEmpty(match.StatusMatch))
        {
            throw new ArgumentException("HomeTeam, VisitingTeam, Championship, Stadium and StatusMatch must not be null or empty.");
        }

        // Constrói a URL
        string url = $"{ApiServiceConstants.urlApi}live/update?id={match.Id}&homeTeam={match.HomeTeam}&visitingTeam={match.VisitingTeam}&championship={match.Championship}&dateMatch={match.DateMatch.ToString("dd/MM/yyyy")}&stadium={match.Stadium}&statusMatch={match.StatusMatch}&gameTime={match.GameTime}&teamPoints1={match.TeamPoints1}&teamPoints2={match.TeamPoints2}&homeTeamWins={match.HomeTeamWins}&visitingTeamWins={match.VisitingTeamWins}&draws={match.Draws}&homeTeamRecentPerformance={match.HomeTeamRecentPerformance}&visitingTeamRecentPerformance={match.VisitingTeamRecentPerformance}&homeTeamOdds={match.HomeTeamOdds}&visitingTeamOdds={match.VisitingTeamOdds}&drawOdds={match.DrawOdds}";

        // Envia a solicitação PUT
        HttpResponseMessage response = await _client.PutAsync(url, null);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsAsync<Live>();
            return result;
        }
        else
        {
            // Handle the error
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error updating entity: {response.StatusCode} - {error}");
        }
    }
}

