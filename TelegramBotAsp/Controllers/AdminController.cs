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

        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates(string topicName)
        {
            return Ok(JsonConvert.SerializeObject(_context.Templates
                .Where(x => x.Topic.Name.ToLower().Equals(topicName.ToLower())).ToList()));
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

        [HttpPost("create-template")]
        public async Task<IActionResult> CreateTemplate(string text, string template, string topicName)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(topicName.ToLower()));
            await _context.Templates.AddAsync(new TextTemplate {Text = text, Template = template, Topic = topic});
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update-template")]
        public async Task<IActionResult> UpdateTemplate(string text, string template, string topicName)
        {
            var templateToUpdate = _context.Templates.FirstOrDefault(x => x.Text.Equals(text));
            if (templateToUpdate != null) templateToUpdate.Template = template;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("delete-template")]
        public async Task<IActionResult> DeleteTemplate(string text, string topicName)
        {
            var templateToRemove = _context.Templates.FirstOrDefault(x => x.Text.Equals(text));
            if (templateToRemove != null) _context.Templates.Remove(templateToRemove);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost("create-topic")]
        public async Task<IActionResult> CreateTopic(string text, string template, string topicName)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(topicName.ToLower()));
            await _context.Templates.AddAsync(new TextTemplate {Text = text, Template = template, Topic = topic});
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}