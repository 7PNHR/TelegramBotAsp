using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using TelegramBotAsp.Services;
using TelegramBotAsp.Models.Admin;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace TelegramBotAsp.Controllers
{
    //TODO написать пару страничек, с нормальным вводом сообщений
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IDataUploadService _dataUploadService;

        public AdminController(IDataUploadService dataUploadService)
        {
            _dataUploadService = dataUploadService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("logs")]
        public IActionResult BotLogs()
        {
            return View(new BotLogsModel(_dataUploadService.GetLogs().Result));
            //return Ok(JsonConvert.SerializeObject(_dataUploadService.GetLogs().Result));
        }

        [HttpGet("logs-period")]
        public IActionResult BotLogsInPeriod(int days)
        {
            var date = DateTime.UtcNow - TimeSpan.FromDays(days);
            return View(new BotLogsModel(_dataUploadService.GetLogs().Result.Where(x => x.Date >= date).ToList()));
            //return Ok(JsonConvert.SerializeObject(_dataUploadService.GetLogs().Result.Where(x => x.Date >= date).ToList()));
        }

        [HttpGet("templates")]
        public IActionResult BotTemplates()
        {
            return View(new BotTemplateModel(_dataUploadService.GetTemplates().Result));
            //return Ok(JsonConvert.SerializeObject(_dataUploadService.GetTemplates().Result));
        }
        
        [HttpGet("edit")]
        public IActionResult Edit()
        {
            return View();
        }
        
        
        [HttpPost("add-template")]
        public IActionResult AddTemplate()
        {
            var request = Request.Form.FirstOrDefault(p => p.Key == "request").Value.ToString();
            var response = Request.Form.FirstOrDefault(p => p.Key == "response").Value.ToString();
            var topicName = Request.Form.FirstOrDefault(p => p.Key == "topicName").Value.ToString();
            _dataUploadService.AddTemplate(request, response, topicName);
            return RedirectToAction("Edit");
            
        }

        [HttpPost("edit-template")]
        public IActionResult EditTemplate()
        {
            var request = Request.Form.FirstOrDefault(p => p.Key == "requestToEdit").Value.ToString();
            var newResponse = Request.Form.FirstOrDefault(p => p.Key == "newResponse").Value.ToString();
            _dataUploadService.EditTemplate(request, newResponse);
            return RedirectToAction("Edit");
        }

        [HttpPost("edit-request")]
        public IActionResult EditResponse()
        {
            var oldRequest = Request.Form.FirstOrDefault(p => p.Key == "oldRequest").Value.ToString();
            var newRequest = Request.Form.FirstOrDefault(p => p.Key == "newRequest").Value.ToString();
            _dataUploadService.EditRequest(oldRequest,newRequest);
            RedirectPermanent("/admin/edit");
            return RedirectToAction("Edit");
        }

        [HttpPost("remove-template")]
        public IActionResult RemoveTemplate()
        {
            var request = Request.Form.FirstOrDefault(p => p.Key == "requestToDelete").Value.ToString();
            _dataUploadService.RemoveTemplate(request);     
            RedirectPermanent("/admin/edit");
            return RedirectToAction("Edit");
        }
    }
}