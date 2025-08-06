using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Ombor.TelegramBot.Application.Handlers;

public static class ErrorHandler
{
    public static Task Handle(
        ITelegramBotClient bot,
        Exception exception,
        CancellationToken token)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiEx =>
                $"API Error:\n[{apiEx.ErrorCode}]\n{apiEx.Message}",
            _ => $"Unknown Error:\n{exception.Message}"
        };

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Error {errorMessage}");
        Console.ResetColor();

        return Task.CompletedTask;
    }
}
