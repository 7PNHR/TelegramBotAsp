using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotAsp.Services
{
    public interface IReplyKeyBoardCreateService
    {
        ReplyKeyboardMarkup CreateInlineKeyBoard(List<string> data);
    }

    public class ReplyKeyBoardCreateService : IReplyKeyBoardCreateService
    {
        public ReplyKeyboardMarkup CreateInlineKeyBoard(List<string> data)
        {
            var list = new List<KeyboardButton[]>();
            list.AddRange(data.Select(text => new [] {new KeyboardButton(text)}).ToList());
            return new ReplyKeyboardMarkup(list);
        }
    }
}