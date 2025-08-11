using Ombor.TelegramBot.Domain.Entities;

namespace Ombor.TelegramBot.Application.Interfaces;
public interface IApiService
{
    Task<Category[]> GetCategoriesAsync();
    Task<Product[]> GetProductsAsync(GetProductsRequest request);
}
