﻿using System.Collections.Generic;
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
        private readonly List<TextTemplate> _templates;
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;
        private readonly ILogService _logService;

        public TextCommand(DataContext context, IUserService userService, TelegramBot telegramBot,
            ILogService logService)
        {
            _templates = context.Templates.ToList();
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
            _logService = logService;
        }

        public override string Name => CommandNames.TextCommand;

        public override async Task ExecuteAsync(Update update)
        {
            foreach (var template in _templates)
            {
                if (update.Message.Text.ToLower().Contains(template.Text.ToLower()))
                {
                    var user = await _userService.GetOrCreate(update);
                    await _botClient.SendTextMessageAsync(user.ChatId, template.Template, ParseMode.Markdown);
                    await _logService.Log(user, update.Message.Text.ToLower());
                }
            }
        }
    }
}