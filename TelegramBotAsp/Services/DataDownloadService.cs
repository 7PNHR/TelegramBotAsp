using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class DataDownloadService : IDataDownloadService
    {
        private readonly IUserService _userService;
        private readonly Dictionary<long, AppUser> _users = new Dictionary<long, AppUser>();
        private Dictionary<string, string> _templates;
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

        public async Task<string> GetTemplate(string template)
        {
            var temp = _templates
                .FirstOrDefault(valuePair => valuePair.Key.ToLower().Equals(template.ToLower()));
            return temp.Value;
        }

        public async Task Update()
        {
            _templates = _context.Templates.ToList()
                .ToDictionary(key => key.Request, element => element.Response);
        }

        public async Task Log(AppUser appUser, string message)
        {
            _context.Logs.Add(new Log {Message = message, User = appUser});
            _context.SaveChanges();
        }

        public async Task<string> GetTopicRequests(string topicName)
        {
            return _context.Templates
                .Where(template => topicName.ToLower().Equals(template.TopicName.ToLower()))
                .ToList()//Странно конечно, когда ленивый метод не хочет доставать данные из базы без вызова ToList()
                .Select(x => x.Request.ToLower())
                .Aggregate((x, y) => x + "\n" + y);
        }
    }
}