using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class HelpCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;
        private readonly ILogService _logService;

        public HelpCommand(IUserService userService, TelegramBot telegramBot, ILogService logService)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
            _logService = logService;
        }

        public override string Name => CommandNames.HelpCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            await _botClient.SendTextMessageAsync(user.ChatId,
                "Вы можете спросить меня про следующие темы:" +
                "\n- оргструктура компании" +
                "\n- документооборот" +
                "\n- ключевые продукты компании" +
                "\n- корпоративная культура" +
                "\n- соц.пакет" +
                "\n- инструменты"+
                "\nПросто напишите название нужной вам темы(ну или нескольких тем через пробел, запятую)." +
                "\nДля ознакомления сразу со всем можете отправить /all",
                ParseMode.Markdown);
            await _logService.Log(user, CommandNames.HelpCommand);

        }
    }
}