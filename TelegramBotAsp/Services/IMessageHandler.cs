using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotAsp.Services
{
    public interface IMessageHandler
    {
        Task Handle(Update update);
    }
}