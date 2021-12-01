using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface ILogService
    {
        Task Log(AppUser appUser, string message);
    }
}