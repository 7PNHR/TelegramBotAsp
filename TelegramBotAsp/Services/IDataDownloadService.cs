using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IDataDownloadService
    {
        Task<AppUser> GetUser(Update update);
        Task<List<string>> GetTemplates(string template);
        Task Update();
        Task Log(AppUser appUser, string message);

        Task<string> GetTopicRequests(string topicName);
        Task<List<string>> GetTopics();
    }
}