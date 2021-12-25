using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly IUserService _userService;
        private readonly Dictionary<long, AppUser> _users = new Dictionary<long, AppUser>();
        private List<Tuple<string, string>> _templates;
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

        public async Task<Tuple<List<string>, string>> GetTemplates(string template)
        {
            if (_requestsTopics.ContainsValue(template) &&
                _requestsTopics.Count(valuePair => Compare(valuePair.Value, template)) != 1)
                return Tuple.Create(_requestsTopics
                    .Where(valuePair => Compare(valuePair.Value, template))
                    .Select(x => HeadLine(x.Key))
                    .ToList(), "Topic");
            return Tuple.Create(_templates
                .Where(valuePair => Compare(valuePair.Item1, template))
                .Select(x => HeadLine(x.Item2))
                .ToList(), "Template");
        }

        public async Task Update()
        {
            _templates = _context.Templates.ToList()
                .Select(x => Tuple.Create(x.Request, x.Response)).ToList();
            _requestsTopics = _context.Templates.ToList().DistinctBy(x => x.Request).ToDictionary(
                key => key.Request,
                element => element.TopicName);
        }

        public async Task Log(AppUser appUser, string message)
        {
            _context.Logs.Add(new Log {Message = message, User = appUser});
            _context.SaveChanges();
        }

        public async Task<string> GetTopicRequests(string topicName)
        {
            return _context.Templates
                .Where(template => Compare(topicName,template.TopicName))
                .ToList() //Странно конечно, когда ленивый метод не хочет доставать данные из базы без вызова ToList()
                .Select(x => HeadLine(x.Request))
                .Aggregate((x, y) => x + "\n" + y);
        }

        public async Task<List<string>> GetTopics()
        {
            return _requestsTopics.Select(x => HeadLine(x.Value)).Distinct().ToList();
        }

        private bool Compare(string first, string second)
        {
            return first.ToLower().Equals(second.ToLower());
        }

        private string HeadLine(string line)
        {
            if (line.Length == 0) return line;
            var result = char.ToUpper(line[0]) + line.Substring(1, line.Length-1);
            return result;
        }
    }
}