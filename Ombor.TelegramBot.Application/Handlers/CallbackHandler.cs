using Microsoft.Extensions.Configuration;
using Ombor.TelegramBot.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ombor.TelegramBot.Application.Handlers;

public class CallbackHandler(ITelegramBotClient client, IApiService apiService, IConfiguration configuration)
{
    public async Task HandleAsync(CallbackQuery callbackQuery)
    {
        if (callbackQuery.Data is null)
        {
            return;
        }

        var data = callbackQuery.Data;
        var chatId = callbackQuery.Message.Chat.Id;

        if (data.StartsWith("product_"))
        {
            var productId = int.Parse(data.Split('_')[1]);
            var product = await apiService.GetProductByIdAsync(productId);

            if (product.Images is not null && product.Images.Count > 0)
            {
                var url = configuration["OmborApi:BaseUrl"];

                List<IAlbumInputMedia> media = new List<IAlbumInputMedia>();

                media.Add(new InputMediaPhoto($"{url}/{product.Images[0].OriginalUrl}")
                {
                    Caption = $"{product.Name}\n\n{product.Description}\n\nЦена: {product.SalePrice} сум"
                });

                for (int i = 1; i < product.Images.Count; i++)
                {
                    media.Add(new InputMediaPhoto($"{url}/{product.Images[i].OriginalUrl}"));
                }

                var addToBasket = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("🛒 Savatga qo'shish", $"add_{productId}")
                    }
                });

                await client.SendMediaGroup(
                    chatId: chatId,
                    media: media
                );

                await client.SendMessage(
                     chatId: chatId,
                     text: "Ushbu mahsulotni savatga qo'shish uchun tugmani bosing:",
                     replyMarkup: addToBasket
                 );
            }
        }
    }
}
