﻿using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;

public interface IApiService<T>
{
    Task<IEnumerable<T>> GetByName(string name);
}

public class ApiService<T> : IApiService<T>
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
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
}
