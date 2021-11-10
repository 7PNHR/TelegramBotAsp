using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace TelegramBotAsp
{
    [ApiController]
    [Route("api/message")]
    public class TelegramBotController : ControllerBase
    {
        // GET
        [HttpPost("update")]
        public IActionResult Update(Update update)
        {
            return Ok();
        }
    }
}