using Microsoft.Extensions.DependencyInjection;
using Ombor.TelegramBot.Application.Interfaces;
using Ombor.TelegramBot.Application.Services;

namespace Ombor.TelegramBot.Application.Extentions;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddScoped<IApiService, ApiService>();

        return services;
    }
}
