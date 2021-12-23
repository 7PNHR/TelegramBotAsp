using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDataDownloadService _dataDownloadService;

        public StartCommand(TelegramBot telegramBot, IDataDownloadService dataDownloadService)
        {
            _botClient = telegramBot.GetBot().Result;
            _dataDownloadService = dataDownloadService;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _dataDownloadService.GetUser(update);
            await _botClient.SendTextMessageAsync(user.ChatId,
                "Добро пожаловать! Я онбординговый бот!" +
                "\nВы можете задавать мне вопросы, касательно организации." +
                "\nДля ознакомления с моими возможностями напишите /help.",
                ParseMode.Markdown);
            _dataDownloadService.Log(user, CommandNames.StartCommand);
        }
    }
}