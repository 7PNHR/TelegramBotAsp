using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotAsp.Entities;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : ControllerBase
    {
        private readonly IMessageHandler _messageHandler;

        public TelegramBotController(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] object update)
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

            var chat = upd.Message?.Chat;
            if (chat == null) return Ok();

            try
            {
                await _messageHandler.Handle(upd);
            }
            catch(Exception exception)
            {
                return Ok();
            }

            return Ok();
        }
    }
}