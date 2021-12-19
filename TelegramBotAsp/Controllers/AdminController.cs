using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TelegramBotAsp.Entities;
using TelegramBotAsp.Services;

namespace TelegramBotAsp.Controllers
{
    //TODO написать пару страничек, с нормальным вводом сообщений
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDataService _dataService;

        public AdminController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetBotLogs()
        {
            return Ok(JsonConvert.SerializeObject(_dataService.GetLogs().Result));
        }
        
        [HttpGet("templates")]
        public async Task<IActionResult> GetBotTemplates()
        {
            return Ok(JsonConvert.SerializeObject(_dataService.GetTemplates().Result));
        }

        [HttpGet("logs-period")]
        public async Task<IActionResult> GetBotLogs(int days)
        {
            var date = DateTime.UtcNow - TimeSpan.FromDays(days);
            return Ok(JsonConvert.SerializeObject(_dataService.GetLogs().Result.Where(x => x.Date >= date).ToList()));
        }

        [HttpPost("add-template")]
        public async Task<IActionResult> AddRequestAndResponse(string request, string response, string topicName)
        {
            await _dataService.AddTemplate(request, response, topicName);
            return Ok();
        }

        [HttpPost("edit-template")]
        public async Task<IActionResult> EditTemplate(string request, string newResponse)
        {
            await _dataService.EditTemplate(request, newResponse);
            return Ok();
        }

        [HttpPost("remove-template")]
        public async Task<IActionResult> RemoveTemplate(string request)
        {
            await _dataService.RemoveTemplate(request);
            return Ok();
        }

        [HttpPost("edit-request")]
        public async Task<IActionResult> EditResponse(string oldRequest, string newRequest)
        {
            await _dataService.EditRequest(oldRequest,newRequest);
            return Ok();
        }

    }
}