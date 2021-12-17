using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IDataService
    {
        public Task AddRequestAndResponse(string request, string response);
        public Task EditRequest(string oldRequest, string newRequest);
        public Task RemoveRequest(string request);
        public Task EditResponse(string oldResponse, string newResponse);
        public Task RemoveResponse(string response);
        public Task<List<Log>> GetLogs();
    }
}