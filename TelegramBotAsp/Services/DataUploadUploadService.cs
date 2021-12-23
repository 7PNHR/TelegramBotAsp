using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class DataUploadUploadService : IDataUploadService
    {
        private readonly DataContext _context;
        private readonly IDataDownloadService _dataDownloadService;

        public DataUploadUploadService(DataContext dataContext, IDataDownloadService dataDownloadService)
        {
            _context = dataContext;
            _dataDownloadService = dataDownloadService;
        }

        public async Task AddTemplate(string request, string response, string topicName)
        {
            await _context.Templates.AddAsync(new TextTemplate {Request = request, Response = response, TopicName = topicName});
            await _context.SaveChangesAsync();
            await _dataDownloadService.Update();
        }

        public async Task EditTemplate(string request, string newResponse)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(
                x => x.Request.ToLower().Equals(request.ToLower()));
            if (template == null) return;
            template.Response = newResponse;
            await _context.SaveChangesAsync();
            await _dataDownloadService.Update();
        }

        public async Task AddRequest(string mainRequest, string newRequest)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(x => x.Request.ToLower().Equals(mainRequest.ToLower()));
            await _context.Templates.AddAsync(new TextTemplate()
                {Request = newRequest, TopicName = template.TopicName, Response = template.Response});
            await _context.SaveChangesAsync();
            await _dataDownloadService.Update();
        }

        public async Task RemoveTemplate(string request)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(
                x => x.Request.ToLower().Equals(request.ToLower()));
            if (template == null) return;
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            await _dataDownloadService.Update();
        }

        public async Task EditRequest(string oldRequest, string newRequest)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(
                x => x.Request.ToLower().Equals(oldRequest.ToLower()));
            if (template == null) return;
            template.Request = newRequest;
            await _context.SaveChangesAsync();
            await _dataDownloadService.Update();
        }

        public async Task<List<Log>> GetLogs()
        {
            return _context.Logs.ToList();
        }
        
        public async Task<List<TextTemplate>> GetTemplates()
        {
            return _context.Templates.ToList();
        }
    }
}