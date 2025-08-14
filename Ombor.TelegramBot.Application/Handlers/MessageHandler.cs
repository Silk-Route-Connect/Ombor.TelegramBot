using Ombor.TelegramBot.Application.Interfaces;
using Ombor.TelegramBot.Domain.Enums;
using Ombor.TelegramBot.Domain.Requests;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ombor.TelegramBot.Application.Handlers;

internal sealed class MessageHandler(ITelegramBotClient bot, IApiService apiService)
{
    private static Dictionary<long, Stack<string>> userStages = new();

    public async Task HandleAsync(Message message)
    {
        if (message.Text is null)
        {
            return;
        }

        var chatId = message.Chat.Id;
        var text = message.Text;

        var categories = await apiService.GetCategoriesAsync();

        switch (text)
        {
            case "/start":
                PushNavigation(chatId, "🏠 Bosh menyu");
                await bot.SendMessage(
                    chatId,
                    "Assalomu alaykum botimizga xush kelibsiz quyidagilardan birini tanlang ");

                await SendMainMenuAsync(chatId);
                break;
            case "/language":
                PushNavigation(chatId, "language");
                await bot.SendMessage(chatId, "hozircha bunaqa funksiya mavjud emas :)");
                break;
            case "🛍 Kategoriyalar":
                PushNavigation(chatId, "🛍 Kategoriyalar");
                await SendCategoriesAsync(chatId);
                break;
            case "🏠 Bosh menyu":
                PushNavigation(chatId, "🏠 Bosh menyu");
                await SendMainMenuAsync(chatId);
                break;
            case "Orqaga":
                var page = PopNavigation(chatId);
                await OpenPage(chatId, page);
                break;
            default:
                if (categories
                    .Select(x => x.Name)
                    .ToList()
                    .Contains(text))
                {
                    var categoryId = categories.FirstOrDefault(x => x.Name.Contains(text))!.Id;
                    await SendProductsAsync(chatId, categoryId, 0);
                    break;
                }
                await bot.SendMessage(chatId, "Siz yuborgan matnni tushunmadim. Iltimos, /start deb yozing.");
                break;
        }
    }

    private async Task OpenPage(long chatId, string page)
    {
        switch (page)
        {
            case "🏠 Bosh menyu":
                await SendMainMenuAsync(chatId);
                break;
            case "categories":
                await SendCategoriesAsync(chatId);
                break;
            case "language":
                break;
            default:
                break;
        }
    }

    private void PushNavigation(long chatId, string page)
    {
        if (!userStages.ContainsKey(chatId))
            userStages[chatId] = new Stack<string>();

        userStages[chatId].Push(page);
    }

    private string PopNavigation(long chatId)
    {
        if (!userStages.ContainsKey(chatId) && userStages[chatId].Count <= 1)
            return null;

        userStages[chatId].Pop();

        return userStages[chatId].Peek();
    }

    private async Task SendCategoriesAsync(long chatId)
    {
        var categories = await apiService.GetCategoriesAsync();

        if (categories is null || categories.Length == 0)
        {
            await bot.SendMessage(chatId, "Kategoriyalar mavjud emas.");
            return;
        }

        var categoryButtons = categories
            .Select(x => new KeyboardButton(x.Name))
            .Chunk(3)
            .Select(row => row.ToArray())
            .ToList();

        categoryButtons.Add(
        [
            new KeyboardButton("Orqaga"),
            new KeyboardButton("🏠 Bosh menyu")
        ]);

        var replyKeyboardMarkup = new ReplyKeyboardMarkup(categoryButtons)
        {
            ResizeKeyboard = true
        };

        await bot.SendMessage(
            chatId: chatId,
            text: $"📦 Kategoriya tanlang:",
            replyMarkup: replyKeyboardMarkup);
    }

    private async Task SendProductsAsync(long chatId, int categoryId, int page)
    {
        var request = new GetProductsRequest
        {
            CategoryId = categoryId,
            Type = ProductType.Sale
        };

        var products = await apiService.GetProductsAsync(request);

        var pageSize = 8;
        var currentPage = page > 0 ? page : 1;
        var totalPages = (int)Math.Ceiling((double)products.Length / pageSize);

        var pagedProducts = products
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var inlineKeyboardButtons = pagedProducts
            .Select(product => InlineKeyboardButton.WithCallbackData(
                text: $"{product.Name} - {product.SalePrice} so'm",
                callbackData: $"product_{product.Id}"))
            .Chunk(2)
            .Select(row => row.ToArray())
            .ToList();

        var paginationButtons = new List<InlineKeyboardButton>();

        if (currentPage > 1)
            paginationButtons.Add(InlineKeyboardButton.WithCallbackData(
                text: "Orqaga",
                callbackData: $"products_{categoryId}_{currentPage - 1}"));

        if (currentPage < totalPages)
            paginationButtons.Add(InlineKeyboardButton.WithCallbackData(
                text: "Keyingi",
                callbackData: $"products_{categoryId}_{currentPage + 1}"));

        if (paginationButtons.Count > 0)
            inlineKeyboardButtons.Add(paginationButtons.ToArray());

        var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyboardButtons);

        await bot.SendMessage(
            chatId: chatId,
            text: $"Kategoriya bo'yicha {products.Length} ta mahsulot topildi.",
            replyMarkup: inlineKeyboard);
    }

    private async Task SendMainMenuAsync(long chatId)
    {
        var mainMenuButtons = new ReplyKeyboardMarkup(
        [
            ["🛍 Kategoriyalar", "🛒 Savatcha"],
            [ "📦 Buyurtmalar", "⚙ Sozlamalar"]
        ])
        {
            ResizeKeyboard = true
        };

        await bot.SendMessage(
            chatId: chatId,
            text: "📋 Asosiy menyu:",
            replyMarkup: mainMenuButtons);
    }
}
