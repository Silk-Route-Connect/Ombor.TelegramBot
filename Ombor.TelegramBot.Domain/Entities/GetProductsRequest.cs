using Ombor.TelegramBot.Domain.Enums;

namespace Ombor.TelegramBot.Domain.Entities;

public class GetProductsRequest
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public ProductType? Type { get; set; }
}
