using System.Text.Json;

namespace Ombor.TelegramBot.Application.Extentions;

public static class Translator
{
    private static Dictionary<string, Dictionary<string, string>> _translations;
    public static Dictionary<long, string> UserLang = new();

    public static void Load(string path)
    {
        var json = File.ReadAllText(path);

        _translations = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize translations.");
    }

    public static string Translate(string key, string language)
    {
        if (_translations.TryGetValue(key, out var langs))
        {
            if (langs.TryGetValue(language, out var value))
                return value;
        }

        return key;
    }
}
