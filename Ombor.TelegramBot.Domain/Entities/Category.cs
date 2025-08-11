using Newtonsoft.Json;

namespace Ombor.TelegramBot.Domain.Entities;
public class Category
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
}
