using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class DataService : IDataService
    {
        private readonly DataContext _context;
        private readonly IRepositoryService _repositoryService;

        public DataService(DataContext dataContext, IRepositoryService repositoryService)
        {
            _context = dataContext;
            _repositoryService = repositoryService;
        }

        public async Task AddRequestAndResponse(string request, string response)
        {
            await _context.Templates.AddAsync(new TextTemplate {Request = request, Response = response});
            await _context.SaveChangesAsync();
            await _repositoryService.Update();
        }

        public async Task EditRequest(string oldRequest, string newRequest)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(
                x => x.Request.ToLower().Equals(oldRequest.ToLower()));
            if (template == null) return;
            template.Request = newRequest;
            await _context.SaveChangesAsync();
            await _repositoryService.Update();
        }

        public async Task RemoveRequest(string request)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(
                x => x.Request.ToLower().Equals(request.ToLower()));
            if (template == null) return;
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            await _repositoryService.Update();
        }

        public async Task EditResponse(string oldResponse, string newResponse)
        {
            var templates = _context.Templates.Where(
                x => x.Response.ToLower().Equals(oldResponse.ToLower())).ToList();
            if (templates.Count == 0) return;
            foreach (var textTemplate in templates)
                textTemplate.Response = newResponse;

            await _context.SaveChangesAsync();
            await _repositoryService.Update();
        }

        public async Task RemoveResponse(string response)
        {
            var templates = _context.Templates.Where(
                x => x.Response.ToLower().Equals(response.ToLower())).ToList();
            if (templates.Count == 0) return;
            _context.Templates.RemoveRange(templates);
            await _context.SaveChangesAsync();
            await _repositoryService.Update();
        }

        public async Task<List<TopicInfo>> GetTopics()
        {
            return _context.Topics.ToList();
        }

        public async Task<List<Log>> GetLogs()
        {
            return _context.Logs.ToList();
        }
    }
}