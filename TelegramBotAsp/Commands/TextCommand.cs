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
        private readonly IRepositoryService _repositoryService;

        public TextCommand(TelegramBot telegramBot, IRepositoryService repositoryService)
        {
            _botClient = telegramBot.GetBot().Result;
            _repositoryService = repositoryService;
        }

        public override string Name => CommandNames.TextCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _repositoryService.GetUser(update);
            var response = await _repositoryService.GetTemplate(update.Message.Text);
            await _botClient.SendTextMessageAsync(user.ChatId, response, ParseMode.Markdown);
            _repositoryService.Log(user, update.Message.Text.ToLower());
        }
    }
}