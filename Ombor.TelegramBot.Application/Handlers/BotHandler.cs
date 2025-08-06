using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace Ombor.TelegramBot.Application.Handlers;

public class BotHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageHandler _messageHandler;
    private readonly CallbackHandler _callbackHandler;

    public BotHandler(ITelegramBotClient client, IServiceProvider serviceProvider)
    {
        _bot = client ?? throw new ArgumentNullException(nameof(client));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _messageHandler = new MessageHandler(_bot, _serviceProvider);
        _callbackHandler = new CallbackHandler(_bot, _serviceProvider);
    }

    public async Task OnUpdate(Update update)
    {
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
