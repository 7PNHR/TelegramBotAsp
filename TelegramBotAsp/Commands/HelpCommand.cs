using System.Linq;
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
        private readonly IDataDownloadService _dataDownloadService;

        public HelpCommand( TelegramBot telegramBot, IDataDownloadService dataDownloadService)
        {
            _botClient = telegramBot.GetBot().Result;
            _dataDownloadService = dataDownloadService;
        }

        public override string Name => CommandNames.HelpCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var info = update.Message.Text.Split(' ');
            var user = await _dataDownloadService.GetUser(update);
            if (info.Length == 1)
            {
                await _botClient.SendTextMessageAsync(user.ChatId,
                    "Вы можете спросить меня про следующие темы:\n- оргструктура компании\n- документооборот" +
                    "\n- ключевые продукты компании\n- корпоративная культура\n- соц.пакет\n- инструменты"+
                    "\nМожете написать /help (название темы) для получения подсказки",
                    ParseMode.Markdown);
                _dataDownloadService.Log(user, CommandNames.HelpCommand);
            }
            else
            {
                await _botClient.SendTextMessageAsync(user.ChatId,await _dataDownloadService.GetTopicRequests(string.Join(' ',info.Skip(1)))
                    ,ParseMode.Markdown);
                _dataDownloadService.Log(user, update.Message.Text.ToLower());
            }
        }
    }
}