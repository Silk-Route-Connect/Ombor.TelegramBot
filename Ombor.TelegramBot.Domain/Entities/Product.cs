using Newtonsoft.Json;

namespace Ombor.TelegramBot.Domain.Entities;

public class Product
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("categoryId")]
    public int CategoryId { get; set; }

    [JsonProperty("categoryName")]
    public string CategoryName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("salePrice")]
    public decimal SalePrice { get; set; }

    [JsonProperty("images")]
    public List<ProductImage> Images { get; set; } = [];
}
