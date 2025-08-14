using Ombor.TelegramBot.Domain.Entities;
using Ombor.TelegramBot.Domain.Requests;

namespace Ombor.TelegramBot.Application.Interfaces;
public interface IApiService
{
    Task<Category[]> GetCategoriesAsync();
    Task<Product[]> GetProductsAsync(GetProductsRequest request);
    Task<Product> GetProductByIdAsync(int productId);
}
