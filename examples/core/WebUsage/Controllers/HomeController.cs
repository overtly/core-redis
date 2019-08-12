using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebUsage.Models;
using Overt.Core.Redis;
using Microsoft.Extensions.Logging;

namespace WebUsage.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var resp = RedisManager.Execute(client => client.StringSet("aaa", "111"));
            _logger.LogInformation($"StringSet result:{resp}");

            var val = RedisManager.Execute<string>(client => client.StringGet("aaa"));
            _logger.LogInformation($"StringGet result:{val}");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
