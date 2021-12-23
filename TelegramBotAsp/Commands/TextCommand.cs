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
            if (responses.Count >= 2)
            {
                var inline = _replyKeyBoard.CreateInlineKeyBoard(responses);
                foreach (var resp in responses)
                    await _botClient.SendTextMessageAsync(user.ChatId, resp, ParseMode.Markdown, replyMarkup: inline);
            }
            else
                await _botClient.SendTextMessageAsync(user.ChatId, responses[0], ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
            await _dataDownloadService.Log(user, text.ToLower());
        }
    }
}