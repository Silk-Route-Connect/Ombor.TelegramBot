using Telegram.Bot;
using Telegram.Bot.Types;

namespace Ombor.TelegramBot.Application.Handlers;

internal sealed class MessageHandler(ITelegramBotClient bot, IServiceProvider serviceProvider)
{
    public async Task HandleAsync(Message message)
    {
        return;
    }
}
