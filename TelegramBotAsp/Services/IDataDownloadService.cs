using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IDataDownloadService
    {
        Task<AppUser> GetUser(Update update);
        Task<string> GetTemplate(string template);
        Task Update();
        Task Log(AppUser appUser, string message);

        Task<string> GetTopicRequests(string topicName);
    }
}