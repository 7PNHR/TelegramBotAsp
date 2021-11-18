using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotAsp.Services
{
    public interface ITextHandler
    {
        Task Handle(Update update);
    }
}