using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TelegramBotAsp.Entities;

namespace TelegramBotAsp.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }
        

        [HttpGet("topics")]
        public async Task<IActionResult> GetTopics()
        {
            return Ok(JsonConvert.SerializeObject(_context.Topics.ToList()));
        }

        [HttpGet("bot-logs")]
        public async Task<IActionResult> GetBotLogs()
        {
            return Ok(JsonConvert.SerializeObject(_context.Logs.ToList()));
        }

        [HttpGet("bot-logs-period")]
        public async Task<IActionResult> GetBotLogs(int days)
        {
            var date = DateTime.UtcNow - TimeSpan.FromDays(days);
            return Ok(JsonConvert.SerializeObject(_context.Logs.Where(x => x.Date >= date).ToList()));
        }
        
    }
}