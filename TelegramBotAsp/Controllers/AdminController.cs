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
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDataService _dataService;

        public AdminController(IDataService dataService)
        {
            _dataService = dataService;
        }
        
        [HttpGet("topics")]
        public async Task<IActionResult> GetTopics()
        {
            return Ok(JsonConvert.SerializeObject(_dataService.GetTopics().Result));
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetBotLogs()
        {
            return Ok(JsonConvert.SerializeObject(_dataService.GetLogs().Result));
        }

        [HttpGet("logs-period")]
        public async Task<IActionResult> GetBotLogs(int days)
        {
            var date = DateTime.UtcNow - TimeSpan.FromDays(days);
            return Ok(JsonConvert.SerializeObject(_dataService.GetLogs().Result.Where(x => x.Date >= date).ToList()));
        }

        [HttpPost("add-request-and-response")]
        public async Task<IActionResult> AddRequestAndResponse(string request, string response)
        {
            await _dataService.AddRequestAndResponse(request, response);
            return Ok();
        }
        
        [HttpPost("edit-request")]
        public async Task<IActionResult> EditRequest(string oldRequest, string newRequest)
        {
            await _dataService.EditRequest(oldRequest, newRequest);
            return Ok();
        }
        
        [HttpPost("remove-request")]
        public async Task<IActionResult> RemoveRequest(string request)
        {
            await _dataService.RemoveRequest(request);
            return Ok();
        }
        
        [HttpPost("edit-response")]
        public async Task<IActionResult> EditResponse(string oldResponse, string newResponse)
        {
            await _dataService.EditResponse(oldResponse,newResponse);
            return Ok();
        }
        
        [HttpPost("remove-response")]
        public async Task<IActionResult> RemoveResponse(string response)
        {
            await _dataService.RemoveResponse(response);
            return Ok();
        }
    }
}