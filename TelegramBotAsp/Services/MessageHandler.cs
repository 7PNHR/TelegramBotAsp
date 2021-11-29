using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Commands;

namespace TelegramBotAsp.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly List<BaseCommand> _commands;

        public MessageHandler(IServiceProvider serviceProvider)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }

        public async Task Handle(Update update)
        {
            if (update?.Message?.Chat == null || update.Message.Text == null) return;
            foreach (var command in _commands)
            {
                if (update.Message != null && update.Message.Text.Contains(command.Name))
                {
                    await ExecuteCommand(command.Name, update);
                    return;
                }
            }
            await ExecuteCommand("text", update);
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            var command = _commands.First(x => x.Name == commandName);
            await command.ExecuteAsync(update);
        }
    }
}
//            return _templates.Find(x => x.Text.ToLower().Equals(text.ToLower()))?.Template;
