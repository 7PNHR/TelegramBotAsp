using System.Threading.Tasks;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class LogService : ILogService
    {
        private readonly DataContext _dataContext;

        public LogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Log(AppUser user, string message)
        {
            await _dataContext.Logs.AddAsync(new Log {Message = message, User = user});
            await _dataContext.SaveChangesAsync();
        }
    }
}