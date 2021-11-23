using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class ParsingService : IParsingService
    {
        
        private List<TextTemplate> _templates;
        private readonly DataContext _context;

        public ParsingService(DataContext context)
        {
            //templates = context.Templates.ToList();
            _context = context;
        }

        public string ParseMessage(string text)
        {
            _templates = _context.Templates.ToList();
            return _templates.Find(x => x.Text.ToLower().Equals(text.ToLower()))?.Template;
        }
    }
    
}