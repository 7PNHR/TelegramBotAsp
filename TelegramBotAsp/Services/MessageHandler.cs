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
        private BaseCommand _lastCommand;
        public MessageHandler(IServiceProvider serviceProvider)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        
        public async Task Handle(Update update)
        {
            if(update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;
            if (update.Message != null && update.Message.Text.Contains(CommandNames.StartCommand))
            {
                await ExecuteCommand(CommandNames.StartCommand, update);
                return;
            }
            if (update.Type == UpdateType.Message)
            {
                
                //TODO сделать сервис поиска ключевых слов
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                //TODO админка!
            }
            
        }
        
        private async Task ExecuteCommand(string commandName, Update update)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update);
        }
    }
}