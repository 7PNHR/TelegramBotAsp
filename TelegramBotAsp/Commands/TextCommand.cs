using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Entities;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class TextCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDataDownloadService _dataDownloadService;

        public TextCommand(TelegramBot telegramBot, IDataDownloadService dataDownloadService)
        {
            _botClient = telegramBot.GetBot().Result;
            _dataDownloadService = dataDownloadService;
        }

        public override string Name => CommandNames.TextCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _dataDownloadService.GetUser(update);
            var response =  _dataDownloadService.GetTemplate(update.Message.Text).Result;
            await _botClient.SendTextMessageAsync(user.ChatId, response, ParseMode.Markdown);
            await _dataDownloadService.Log(user, update.Message.Text.ToLower());
        }
    }
}