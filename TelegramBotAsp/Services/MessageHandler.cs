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
        //private readonly ITextHandler _textHandler;
        private BaseCommand _lastCommand;

        public MessageHandler(IServiceProvider serviceProvider, ITextHandler textHandler)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
            //_textHandler = textHandler;
        }

        public async Task Handle(Update update)
        {
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;
            if (update.Message != null && update.Message.Text.Contains(CommandNames.StartCommand))
            {
                await ExecuteCommand(CommandNames.StartCommand, update);
                return;
            }

            if (update.Type == UpdateType.Message)
                return;
                //await _textHandler.Handle(update);

            //var text = _parsingService.ParseMessage(update.Message?.Text);

            if (update.Type == UpdateType.CallbackQuery)
            {
                //TODO админка!
            }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            //_lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update);
        }
    }
}