using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApplication1.Models;
using static WebApplication1.Startup;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITest1 test2;
        public HomeController(ITest1 test)
        {
            this.test2 = test;
        }
        public IActionResult Index()
        {
            Console.WriteLine(test2);
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
