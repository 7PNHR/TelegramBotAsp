using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(AppUser user, string text);
    }
}