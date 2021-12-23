using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IDataUploadService
    {
        public Task AddTemplate(string request, string response, string topicName);
        public Task EditRequest(string oldRequest, string newRequest);

        public Task EditTemplate(string request, string newResponse);
        public Task AddRequest(string mainRequest, string newRequest);
        public Task RemoveTemplate(string request);
        public Task<List<Log>> GetLogs();
        public Task<List<TextTemplate>> GetTemplates();
    }
}