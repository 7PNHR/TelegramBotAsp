using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotAsp.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update);
    }
}