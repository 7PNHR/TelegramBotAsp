using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public interface IReplyKeyBoardCreateService
    {
        ReplyKeyboardMarkup CreateInlineKeyBoard(AppUser user, List<string> data);
        ReplyKeyboardMarkup GetPreviousKeyBoard(AppUser user);
    }

    public class ReplyKeyBoardCreateService : IReplyKeyBoardCreateService
    {
        private readonly Dictionary<long, Stack<ReplyKeyboardMarkup>> _keyBoards =
            new Dictionary<long, Stack<ReplyKeyboardMarkup>>();

        public ReplyKeyboardMarkup CreateInlineKeyBoard(AppUser user, List<string> data)
        {
            var list = new List<KeyboardButton[]>();
            list.AddRange(data.Select(text => new[] {new KeyboardButton(text)}).ToList());
            list.Add(new[] {new KeyboardButton("Вернуться назад")});
            var result = new ReplyKeyboardMarkup(list);
            if (!_keyBoards.ContainsKey(user.ChatId))
                _keyBoards.Add(user.ChatId, new Stack<ReplyKeyboardMarkup>());
            _keyBoards[user.ChatId].Push(result);
            return result;
        }

        public ReplyKeyboardMarkup GetPreviousKeyBoard(AppUser user)
        {
            var check = _keyBoards.ContainsKey(user.ChatId);
            if (check && _keyBoards[user.ChatId].Count >1)
            {
                _keyBoards[user.ChatId].Pop();
                return _keyBoards[user.ChatId].Peek();
            }
            throw new Exception();
        }
    }
}