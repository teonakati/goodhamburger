using goodhamburger.Application.DTOs;
using System.Net.Http.Json;

namespace goodhamburger.Client.Services;

public class ApiClient(HttpClient http)
{
    // ── Products ────────────────────────────────────────────────────────────

    public Task<List<ProductDto>?> GetProductsAsync() =>
        http.GetFromJsonAsync<List<ProductDto>>("api/product");

    public Task<ProductDto?> GetProductAsync(int id) =>
        http.GetFromJsonAsync<ProductDto>($"api/product/{id}");

    public Task<HttpResponseMessage> CreateProductAsync(CreateProductDto dto) =>
        http.PostAsJsonAsync("api/product", dto);

    public Task<HttpResponseMessage> UpdateProductAsync(int id, CreateProductDto dto) =>
        http.PutAsJsonAsync($"api/product/{id}", dto);

    public Task<HttpResponseMessage> DeleteProductAsync(int id) =>
        http.DeleteAsync($"api/product/{id}");

    // ── Orders ──────────────────────────────────────────────────────────────

    public Task<List<OrderDto>?> GetOrdersAsync() =>
        http.GetFromJsonAsync<List<OrderDto>>("api/order");

    public Task<OrderDto?> GetOrderAsync(int id) =>
        http.GetFromJsonAsync<OrderDto>($"api/order/{id}");

    public Task<HttpResponseMessage> CreateOrderAsync(CreateOrderDto dto) =>
        http.PostAsJsonAsync("api/order", dto);

    public Task<HttpResponseMessage> UpdateOrderAsync(int id, CreateOrderDto dto) =>
        http.PutAsJsonAsync($"api/order/{id}", dto);

    public Task<HttpResponseMessage> DeleteOrderAsync(int id) =>
        http.DeleteAsync($"api/order/{id}");
}
