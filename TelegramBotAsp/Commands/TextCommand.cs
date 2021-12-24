using System;
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
            var (responses, type) = _dataDownloadService.GetTemplates(text).Result;
            IReplyMarkup inline = null;
            if (type.Equals("Topic"))
            {
                inline = _replyKeyBoard.CreateInlineKeyBoard(user, responses);
                responses.Reverse();
                responses.Add("В этом разделе ты можешь узнать:");
                responses.Reverse();
            }
            await _botClient.SendTextMessageAsync(user.ChatId,
                string.Join('\n', responses),
                ParseMode.Markdown,
                replyMarkup: inline);
            await _dataDownloadService.Log(user, text.ToLower());
        }
    }
}