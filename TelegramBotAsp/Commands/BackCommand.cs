using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotAsp.Entities;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Commands
{
    public class BackCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IReplyKeyBoardCreateService _replyKeyBoard;

        public BackCommand(TelegramBot telegramBot, IReplyKeyBoardCreateService replyKeyBoard)
        {
            _botClient = telegramBot.GetBot().Result;
            _replyKeyBoard = replyKeyBoard;
        }

        public override string Name => CommandNames.BackCommand;

        public override async Task ExecuteAsync(AppUser user, string text)
        {
            try
            {
                IReplyMarkup inline = _replyKeyBoard.GetPreviousKeyBoard(user);
                await _botClient.SendTextMessageAsync(user.ChatId, "Вы вернулись назад", replyMarkup: inline);
            }
            catch (Exception e)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "Вы не можете вернуться назад");
            }
        }
    }
}