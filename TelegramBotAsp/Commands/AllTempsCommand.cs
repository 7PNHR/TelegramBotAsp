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
    public class AllTempsCommand : BaseCommand
    {
        private readonly List<TextTemplate> _templates;
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;
        private readonly ILogService _logService;


        public AllTempsCommand(DataContext context, IUserService userService, TelegramBot telegramBot,
            ILogService logService)
        {
            _templates = context.Templates.ToList();
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
            _logService = logService;
        }

        public override string Name => CommandNames.AllTempsCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            foreach (var template in _templates)
                await _botClient.SendTextMessageAsync(user.ChatId, template.Template, ParseMode.Markdown);
            await _logService.Log(user, CommandNames.AllTempsCommand);
        }
    }
}