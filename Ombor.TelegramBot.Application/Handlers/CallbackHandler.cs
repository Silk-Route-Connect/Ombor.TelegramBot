using Telegram.Bot;
using Telegram.Bot.Types;

namespace Ombor.TelegramBot.Application.Handlers;

public class CallbackHandler(ITelegramBotClient client, IServiceProvider serviceProvider)
{
    public async Task HandleAsync(CallbackQuery callbackQuery)
    {
        return;
    }
}
