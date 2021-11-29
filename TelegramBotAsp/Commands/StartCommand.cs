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

        public StartCommand(IUserService userService, TelegramBot telegramBot)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            await _botClient.SendTextMessageAsync(user.ChatId,
                "Добро пожаловать! Я онбординговый бот!\nВы можете задавать мне вопросы, касательно организации. " +
                "Вы можете спросить меня про следующие темы:\n- оргструктура компании\n- документооборот\n- ключевые продукты компании" +
                "\n- корпоративная культура\n- соц.пакет\n- инструменты",
                ParseMode.Markdown);
        }
    }
}