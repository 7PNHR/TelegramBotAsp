using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IRepositoryService
    {
        Task<AppUser> GetUser(Update update);
        Task<string> GetTemplate(string template);
        Task<string> GetTopicInfo(string text);
        Task Update();
        Task Log(AppUser appUser, string message);
        
    }
}