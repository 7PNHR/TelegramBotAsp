using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IUserService
    {
        Task<AppUser> GetOrCreate(Update update);
    }
}