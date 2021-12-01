using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;
        private readonly ILogService _logService;

        public StartCommand(IUserService userService, TelegramBot telegramBot, ILogService logService)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
            _logService = logService;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            await _botClient.SendTextMessageAsync(user.ChatId,
                "Добро пожаловать! Я онбординговый бот!" +
                "\nВы можете задавать мне вопросы, касательно организации." +
                "\nДля ознакомления с моими возможностями напишите /help.",
                ParseMode.Markdown);
            await _logService.Log(user, CommandNames.StartCommand);
        }
    }
}