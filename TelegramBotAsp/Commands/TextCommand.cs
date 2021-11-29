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
        private readonly DataContext _context;
        private readonly List<TextTemplate> _templates;
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public TextCommand(DataContext context,IUserService userService, TelegramBot telegramBot)
        {
            _context = context;
            _templates = context.Templates.ToList();
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }
        public override string Name => CommandNames.HandleCommand;
        public override async Task ExecuteAsync(Update update)
        {
            foreach (var template in _templates)
            {
                if (update.Message.Text.Contains(template.Text))
                {
                    var user = await _userService.GetOrCreate(update);
                    await _botClient.SendTextMessageAsync(user.ChatId,template.Template,ParseMode.Markdown);
                    return;
                }
            }
        }
    }
}