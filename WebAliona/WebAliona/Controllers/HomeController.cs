using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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
        public async Task<IActionResult> Create(Banan banan, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(folderPath);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(folderPath, fileName);
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    await Image.CopyToAsync(fileStream);
                //}
                using var stream = image.OpenReadStream();
                using var newImage = await Image.LoadAsync(stream); // ImageSharp завантажує з потоку

                newImage.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(800, 600),
                    Mode = ResizeMode.Max
                }));

                await newImage.SaveAsync(filePath); // автоматично визначає формат за розширенням

                banan.Image = fileName;
            }

            //var fileName = await SaveFile(banan.Image);
            //banan.Image = fileName;
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
