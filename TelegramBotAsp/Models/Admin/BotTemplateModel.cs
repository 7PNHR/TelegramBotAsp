using System.Collections.Generic;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Models.Admin
{
    public class BotTemplateModel
    {
        public List<TextTemplate> Templates { get; }

        public BotTemplateModel(List<TextTemplate> template)
        {
            Templates = template;
        }
    }
}