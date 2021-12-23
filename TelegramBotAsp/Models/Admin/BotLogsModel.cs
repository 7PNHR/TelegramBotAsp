using System.Collections.Generic;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Models.Admin
{
    public class BotLogsModel
    {
        public List<Log> Logs { get; }

        public BotLogsModel(List<Log> logs)
        {
            Logs = logs;
        }
    }
}