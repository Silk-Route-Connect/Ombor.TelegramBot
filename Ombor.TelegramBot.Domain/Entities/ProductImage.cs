namespace Ombor.TelegramBot.Domain.Entities;
public class ProductImage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
}
