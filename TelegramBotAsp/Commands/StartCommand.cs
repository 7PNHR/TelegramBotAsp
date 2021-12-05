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
        private readonly IRepositoryService _repositoryService;

        public StartCommand(TelegramBot telegramBot, IRepositoryService repositoryService)
        {
            _botClient = telegramBot.GetBot().Result;
            _repositoryService = repositoryService;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _repositoryService.GetUser(update);
            await _botClient.SendTextMessageAsync(user.ChatId,
                "Добро пожаловать! Я онбординговый бот!" +
                "\nВы можете задавать мне вопросы, касательно организации." +
                "\nДля ознакомления с моими возможностями напишите /help.",
                ParseMode.Markdown);
            _repositoryService.Log(user, CommandNames.StartCommand);
        }
    }
}