using System.Collections.Generic;
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
    public class TextCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDataDownloadService _dataDownloadService;
        private readonly IReplyKeyBoardCreateService _replyKeyBoard;

        public TextCommand(TelegramBot telegramBot, IDataDownloadService dataDownloadService,
            IReplyKeyBoardCreateService replyKeyBoard)
        {
            _botClient = telegramBot.GetBot().Result;
            _dataDownloadService = dataDownloadService;
            _replyKeyBoard = replyKeyBoard;
        }

        public override string Name => CommandNames.TextCommand;

        public override async Task ExecuteAsync(AppUser user, string text)
        {
            var responses = _dataDownloadService.GetTemplates(text).Result;
            IReplyMarkup inline;
            if (responses.Item2.Equals("Topic"))
                inline = _replyKeyBoard.CreateInlineKeyBoard(responses.Item1);
            else
                inline = new ReplyKeyboardRemove();
            foreach (var resp in responses.Item1)
                await _botClient.SendTextMessageAsync(user.ChatId, resp, ParseMode.Markdown, replyMarkup: inline);
            await _dataDownloadService.Log(user, text.ToLower());
        }
    }
}