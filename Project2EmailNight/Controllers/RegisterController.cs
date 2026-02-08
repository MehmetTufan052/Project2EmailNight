using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegisterDto userRegisterDto)
        {
            Random random = new Random();
            int confirmCode = random.Next(100000, 999999);

            AppUser appUser = new AppUser()
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                ConfirmCode = confirmCode
            };

            var result = await _userManager.CreateAsync(appUser, userRegisterDto.Password);

            if (result.Succeeded)
            {
                // 📧 BURADA MAIL GÖNDERİLECEK
                // Email: userRegisterDto.Email
                // İçerik: confirmCode

                // Örnek mesaj (şimdilik)
                TempData["MailInfo"] = "Doğrulama kodunuz email adresinize gönderildi.";

                return RedirectToAction("EmailConfirm");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View();
        }

        public IActionResult EmailConfirmed()
        {
            return View();
        }

    }
}
