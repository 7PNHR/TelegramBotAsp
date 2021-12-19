using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly IUserService _userService;
        private readonly Dictionary<long, AppUser> _users = new Dictionary<long, AppUser>();
        private Dictionary<string, string> _templates;
        private readonly DataContext _context;
        private readonly List<Log> _logs = new List<Log>();


        public RepositoryService(IUserService userService, DataContext context)
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

        public async Task<string> GetTemplate(string template)
        {
            var temp = _templates
                .FirstOrDefault(valuePair => valuePair.Key.ToLower().Equals(template.ToLower()));
            return temp.Value;
        }

        public async Task Update()
        {
            _templates = _context.Templates
                .ToDictionary(key => key.Request, element => element.Response);
        }

        public async Task Log(AppUser appUser, string message)
        {
            _logs.Add(new Log {Message = message, User = appUser});
            lock (_logs)
            {
                if (_logs.Count >= 10)
                {
                    foreach (var log in _logs)
                        _context.Logs.Add(log);

                    _context.SaveChanges();
                    _logs.Clear();
                }
            }
        }

        public async Task<string> GetTopicRequests(string topicName)
        {
            return _context.Templates
                .Where(template => topicName.ToLower().Equals(template.TopicName.ToLower()))
                .ToList()//Странно конечно, когда ленивый метод не хочет доставать данные из базы без вызова ToList()
                .Select(x => x.Request)
                .Aggregate((x, y) => x + "\n" + y);
        }
    }
}