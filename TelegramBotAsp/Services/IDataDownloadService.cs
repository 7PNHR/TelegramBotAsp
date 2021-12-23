using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IDataDownloadService
    {
        Task<AppUser> GetUser(Update update);
        Task<Tuple<List<string>,string>> GetTemplates(string template);
        Task Update();
        Task Log(AppUser appUser, string message);

        Task<string> GetTopicRequests(string topicName);
        Task<List<string>> GetTopics();
    }
}