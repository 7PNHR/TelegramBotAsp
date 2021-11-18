using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotAsp.Services
{
    public class TextHandler : ITextHandler
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public TextHandler(IUserService userService, TelegramBot telegramBot)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }
        
        public Task Handle(Update update)
        {
            
        }
    }
}