using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Services
{
    public class UserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetOrCreate(Update update)
        {
            var newUser = new AppUser()
            {
                Username = update.Message.Chat.Username,
                ChatId = update.Message.Chat.Id,
                FirstName = update.Message.Chat.FirstName,
                LastName = update.Message.Chat.LastName
            };
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);
            if (user != null) return user;
            var result = await _context.Users.AddAsync(newUser);

            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}