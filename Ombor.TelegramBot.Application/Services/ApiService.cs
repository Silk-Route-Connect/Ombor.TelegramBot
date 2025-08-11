using Microsoft.Extensions.Configuration;
using Ombor.TelegramBot.Application.Interfaces;
using Ombor.TelegramBot.Domain.Entities;
using System.Text.Json;

namespace Ombor.TelegramBot.Application.Services;

internal class ApiService(HttpClient httpClient, IConfiguration configuration) : IApiService
{
    public async Task<Category[]> GetCategoriesAsync()
    {
        var url = configuration["OmborApi:BaseUrl"] + "/categories";
        var response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<Category[]>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });

        return categories ?? [];
    }

    public async Task<Product[]> GetProductsAsync(GetProductsRequest request)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            queryParams.Add($"SearchTerm={Uri.EscapeDataString(request.SearchTerm)}");

        if (request.CategoryId.HasValue)
            queryParams.Add($"CategoryId={request.CategoryId.Value}");

        if (request.MinPrice.HasValue)
            queryParams.Add($"MinPrice={request.MinPrice.Value}");

        if (request.MaxPrice.HasValue)
            queryParams.Add($"MaxPrice={request.MaxPrice.Value}");

        if (request.Type.HasValue)
            queryParams.Add($"Type={request.Type.Value}");

        var queryString = string.Join("&", queryParams);

        var url = configuration["OmborApi:BaseUrl"] + "/products";

        if (queryParams.Count > 0)
        {
            url += "?" + queryString;
        }

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<Product[]>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        });

        return products ?? [];
    }
}
