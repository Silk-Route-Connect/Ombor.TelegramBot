using Microsoft.Extensions.Configuration;
using Ombor.TelegramBot.Application.Extentions;
using Ombor.TelegramBot.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace Ombor.TelegramBot.Application.Handlers;

public class BotHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly MessageHandler _messageHandler;
    private readonly CallbackHandler _callbackHandler;
    private readonly IApiService _apiService;
    private readonly IConfiguration _configuration;

    public BotHandler(ITelegramBotClient client, IConfiguration configuration, IApiService apiService)
    {
        _bot = client ?? throw new ArgumentNullException(nameof(client));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _messageHandler = new MessageHandler(_bot, _apiService);
        _callbackHandler = new CallbackHandler(_bot, _apiService, _configuration);
    }

    public async Task OnUpdate(Update update)
    {
        if (update.Message is null)
        {
            if (!Translator.UserLang.TryGetValue(update.CallbackQuery!.Message!.Chat.Id, out var lang))
            {
                Translator.UserLang[update.CallbackQuery.Message.Chat.Id] = "uz";
            }
        }
        else
        {
            if (!Translator.UserLang.TryGetValue(update.Message.Chat.Id, out var lang))
            {
                Translator.UserLang[update.Message.Chat.Id] = "uz";
            }
        }


        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message is not null)
                {
                    await _messageHandler.HandleAsync(update.Message);
                }
                break;
            case UpdateType.CallbackQuery:
                if (update.CallbackQuery is not null)
                {
                    await _callbackHandler.HandleAsync(update.CallbackQuery);
                }
                break;
        }
    }
}
