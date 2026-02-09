using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmMailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailDto confirmMailDto)
        {
            var user=await _userManager.FindByEmailAsync(confirmMailDto.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı.");
                return View();
            }

            if (user.ConfirmCode == confirmMailDto.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                
                return RedirectToAction("Index", "Login");
            }
            ModelState.AddModelError("", "Doğrulama kodu hatalı.");
            return View();
           
        }
    }
}
