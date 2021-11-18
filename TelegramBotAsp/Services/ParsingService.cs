using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class ParsingService : IParsingService
    {
        
        private readonly List<TextTemplate> _templates;

        public ParsingService(DataContext context)
        {
            _templates = context.Templates.ToList();
        }

        public string ParseMessage(string text)
            => _templates.Find(x => x.Text.ToLower().Equals(text.ToLower()))?.Template;
    }
    
}