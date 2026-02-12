using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2EmailNight.Context;

namespace Project2EmailNight.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EmailContext _context;

        public CategoryController(EmailContext context)
        {
            _context = context;
        }

        public IActionResult WorkCategory()
        {

            {
               
                var values = _context.Messages
                                     .Where(x => x.EmailCategory.Name == "İş Epostaları")
                                     .OrderByDescending(x => x.SendDate)
                                     .ToList();

                return View(values);
            }
        }
        public IActionResult EgitimEposta()
        {
            return View();
        }
        public IActionResult AileEposta()
        {
            return View();
        }
    }
}
