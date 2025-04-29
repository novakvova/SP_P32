using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SPExamen.Models;

namespace SPExamen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            DriverViewModel model = new DriverViewModel();
            //DriveInfo[] listDrive = DriveInfo.GetDrives();

            //foreach (DriveInfo drive in listDrive)
            //{
            //    model.Drivers.Add(drive.Name);
            //}

            var drive = "D:\\";
            model.Drivers = Directory.GetDirectories(drive).ToList();
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ScanOptions options)
        {


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
