using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class HelpCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IRepositoryService _repositoryService;

        public HelpCommand( TelegramBot telegramBot, IRepositoryService repositoryService)
        {
            _botClient = telegramBot.GetBot().Result;
            _repositoryService = repositoryService;
        }

        public override string Name => CommandNames.HelpCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var info = update.Message.Text.Split(' ');
            var user = await _repositoryService.GetUser(update);
            if (info.Length != 2)
            {
                await _botClient.SendTextMessageAsync(user.ChatId,
                    "Вы можете спросить меня про следующие темы:\n- оргструктура компании\n- документооборот" +
                    "\n- ключевые продукты компании\n- корпоративная культура\n- соц.пакет\n- инструменты"+
                    "\nМожете написать /help (название темы) для получения подсказки",
                    ParseMode.Markdown);
                _repositoryService.Log(user, CommandNames.HelpCommand);
            }
            else
            {
                await _botClient.SendTextMessageAsync(user.ChatId,await _repositoryService.GetTopicInfo(info[1])
                    ,ParseMode.Markdown);
                _repositoryService.Log(user, update.Message.Text.ToLower());
            }
        }
    }
}