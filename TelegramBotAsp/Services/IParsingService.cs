#nullable enable
namespace TelegramBotAsp.Services
{
    public interface IParsingService
    {
        public string ParseMessage(string? messageText);
    }
}