using System.Diagnostics;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using OrderingMicroS.Models;

namespace OrderingMicroS.Controllers
{
    public class HomeController : Controller
    {
        readonly IEventBus _eventBus;

        public HomeController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public IActionResult Index()
        {
            //get all catalog items, to know what to order
            GetCatalogItems jsonToBe = new GetCatalogItems
            {
                Action = Action.GetAll
            };
           _eventBus.Publish(jsonToBe, "Catalog");

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
