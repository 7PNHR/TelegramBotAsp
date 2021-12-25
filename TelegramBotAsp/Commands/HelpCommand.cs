using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotAsp.Entities;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class HelpCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDataDownloadService _dataDownloadService;
        private readonly IReplyKeyBoardCreateService _replyKeyBoard;

        public HelpCommand(TelegramBot telegramBot, IDataDownloadService dataDownloadService,
            IReplyKeyBoardCreateService replyKeyBoard)
        {
            _botClient = telegramBot.GetBot().Result;
            _dataDownloadService = dataDownloadService;
            _replyKeyBoard = replyKeyBoard;
        }

        public override string Name => CommandNames.HelpCommand;

        public override async Task ExecuteAsync(AppUser user, string text)
        {
            var info = text.Split(' ');
            if (info.Length == 1)
            {
                var topics = await _dataDownloadService.GetTopics();
                await _botClient.SendTextMessageAsync(user.ChatId,
                    "Вы можете спросить меня про следующие темы:\n- Оргструктура компании\n- Документооборот" +
                    "\n- Ключевые продукты компании\n- Корпоративная культура\n- Соц.пакет\n- Инструменты\n- Рабочее окружение" +
                    "\nМожете написать /help (название темы) для получения подсказки",
                    ParseMode.Markdown,
                    replyMarkup: _replyKeyBoard.CreateInlineKeyBoard(user, topics));
                _dataDownloadService.Log(user, CommandNames.HelpCommand);
            }
            else
            {
                var responses = _dataDownloadService.GetTemplates((string.Join(' ', info.Skip(1)))).Result;
                var reply = _replyKeyBoard.CreateInlineKeyBoard(user, responses.Item1);
                await _botClient.SendTextMessageAsync(user.ChatId,
                    await _dataDownloadService.GetTopicRequests(string.Join(' ', info.Skip(1))),
                    ParseMode.Markdown, replyMarkup: reply);
                _dataDownloadService.Log(user, text.ToLower());
            }
        }
    }
}