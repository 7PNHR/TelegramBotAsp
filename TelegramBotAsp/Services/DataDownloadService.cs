using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly IUserService _userService;
        private readonly Dictionary<long, AppUser> _users = new Dictionary<long, AppUser>();
        private Dictionary<string, string> _templates;
        private Dictionary<string, string> _requestsTopics;
        private readonly DataContext _context;


        public DataDownloadService(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
            Update();
        }

        public async Task<AppUser> GetUser(Update update)
        {
            if (update.Message != null && _users.ContainsKey(update.Message.Chat.Id))
                return _users[update.Message.Chat.Id];
            var user = await _userService.GetOrCreate(update);
            _users.Add(user.ChatId, user);
            return user;
        }

        public async Task<List<string>> GetTemplates(string template)
        {
            var count = _requestsTopics.Count(valuePair => valuePair.Value.Equals(template)) != 1;
            var check = _requestsTopics.ContainsValue(template);
            if (check && count)
                return _requestsTopics
                    .Where(valuePair => valuePair.Value.Equals(template))
                    .Select(x => x.Key)
                    .ToList();
            return _templates
                .Where(valuePair => valuePair.Key.Equals(template))
                .Select(x => x.Value)
                .ToList();
        }

        public async Task Update()
        {
            _templates = _context.Templates.ToList().ToDictionary(key => key.Request.ToLower(), 
                    element => element.Response.ToLower());
            _requestsTopics = _context.Templates.ToList().ToDictionary(key => key.Request.ToLower(), 
                    element => element.TopicName.ToLower());
            var check = 0;
        }

        public async Task Log(AppUser appUser, string message)
        {
            _context.Logs.Add(new Log {Message = message, User = appUser});
            _context.SaveChanges();
        }

        public async Task<string> GetTopicRequests(string topicName)
        {
            return _context.Templates
                .Where(template => topicName.Equals(template.TopicName))
                .ToList() //Странно конечно, когда ленивый метод не хочет доставать данные из базы без вызова ToList()
                .Select(x => x.Request.ToLower())
                .Aggregate((x, y) => x + "\n" + y);
        }

        public async Task<List<string>> GetTopics()
        {
            return _requestsTopics.Select(x => x.Value).Distinct().ToList();
        }
    }
}