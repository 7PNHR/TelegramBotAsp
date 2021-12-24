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
        private readonly IDataDownloadService _dataDownloadService;

        public MessageHandler(IServiceProvider serviceProvider, IDataDownloadService dataDownloadService)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
            _dataDownloadService = dataDownloadService;
        }

        public async Task Handle(Update update)
        {
            if (update?.Message?.Chat is null || (update.Message.Text is null && update.CallbackQuery is null)) return;
            foreach (var command in _commands)
            {
                if (update.Message != null && update.Message.Text.Contains(command.Name))
                {
                    await ExecuteCommand(command.Name, update.Message.Text, update);
                    return;
                }
            }
            /*if (update.Type == UpdateType.CallbackQuery)
            {
                await ExecuteCommand("text", update.CallbackQuery.Data, update);
                return;
            }*/
            await ExecuteCommand("text", update.Message.Text, update);
        }

        private async Task ExecuteCommand(string commandName, string commandText, Update update)
        {
            var command = _commands.First(x => x.Name == commandName);
            var user = await _dataDownloadService.GetUser(update);
            await command.ExecuteAsync(user, commandText.ToLower());
        }
    }
}