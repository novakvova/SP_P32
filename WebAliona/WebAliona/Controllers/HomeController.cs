using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAliona.Data;
using WebAliona.Models;

namespace WebAliona.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppAlionaContext _context;

        public HomeController(ILogger<HomeController> logger,
            AppAlionaContext context)
        {
            _logger = logger;
            _context = context;
            if (!_context.Banans.Any()) 
            {
                DataBaseManager dbm = new DataBaseManager();
                dbm.AddBanans(_context);
            }
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Thread main id " + Thread.CurrentThread.ManagedThreadId);
            List<Banan> list = await GetListBanansAsync();
            //Task<List<Banan>> list = GetListBanansAsync();
            return View(list);
        }

        [HttpGet] //Цей метод буде відображати сторінку де можвка вказаи інформацію про користувача
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //зберігає дані від користувача
        public async Task<IActionResult> Create(Banan banan)
        {
            await _context.AddAsync(banan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Task<List<Banan>> GetListBanansAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Child Thread: {Thread.CurrentThread.ManagedThreadId}");
                return _context.Banans.ToList();
            });
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
