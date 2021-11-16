using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly DataContext _context;

        public TelegramBotController(TelegramBot telegramBot, DataContext context)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
            _context = context;
        }

        // GET
        //("update")
        [HttpPost]
        public async Task<IActionResult> Update([FromBody]object update)
        {
            // /start => reg user

            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
            var chat = upd.Message?.Chat;
            if (chat == null) return Ok();

            var appUser = new AppUser()
            {
                Username = chat?.Username,
                ChatId = chat.Id,
                FirstName = chat?.FirstName,
                LastName = chat?.LastName
            };

            var result = await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();

            await _telegramBotClient.SendTextMessageAsync(chat.Id, "Вы успешно зарегались", ParseMode.Markdown);
            
            return Ok();
        }
    }
}