using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ombor.TelegramBot.Application.Handlers;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration(config =>
     {
         config.SetBasePath(Directory.GetCurrentDirectory());
         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
     })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
        }
    })
    .Build();

var bot = host.Services.GetRequiredService<ITelegramBotClient>();
var botHandler = host.Services.GetRequiredService<BotHandler>();

bot!.StartReceiving(
    updateHandler: async (botClient, update, cancellationToken) =>
    {
        if (update != null)
            await botHandler.OnUpdate(update);
    },
    errorHandler: ErrorHandler.Handle
);

Console.WriteLine("Bot ishga tushdi...");
await host.RunAsync();
await Task.Delay(-1);