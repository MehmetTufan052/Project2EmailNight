using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
                return View(userLoginDto);

            var user = await _userManager.FindByNameAsync(userLoginDto.Username);

            // 1️⃣ Kullanıcı yoksa
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(userLoginDto);
            }

            // 2️⃣ Şifre doğru mu?
            var passwordCorrect = await _userManager.CheckPasswordAsync(
                user, userLoginDto.Password);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(userLoginDto);
            }

            // 3️⃣ Email onaylı mı?
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Lütfen email adresinizi onaylayınız.");
                return View(userLoginDto);
            }

            // 4️⃣ Giriş
            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Index", "Profile");
        }
    }
}
