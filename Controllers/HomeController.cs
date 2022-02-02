using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using(MainOffice mainOffice = new MainOffice(3))
            {
                return View(mainOffice.Dealerships);
            }
        }
        //[HttpPost]
        public IActionResult CarItemsIndex(int officeId)
        {
            using (MainOffice mainOffice = new MainOffice(3))
            {
                ViewBag.OfficeId = officeId;
                Office office = mainOffice.Dealerships.FirstOrDefault(x => x.OfficeId == officeId);
                if (office == null)
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction("CarItemsIndex", "CarItem", new { officeid = office.OfficeId });
                }
            }
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
