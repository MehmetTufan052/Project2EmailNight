using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project2EmailNight.Context;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public ProfileController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("UserLogin", "Login");

            var lastMessages = await _context.Messages
        .Where(x => x.ReceiverEmail == user.Email && !x.IsDeleted)
        .OrderByDescending(x => x.SendDate)
        .Take(5)
        .ToListAsync();

            var dto = new UserEditDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
            ? "/assets/images/avatars/avatar-1.png"
            : user.ImageUrl,

                DraftCount = await _context.Messages
            .CountAsync(x => x.SenderEmail == user.Email && x.IsDraft),

                StarredCount = await _context.Messages
            .CountAsync(x => x.ReceiverEmail == user.Email && x.IsStarred),

                LastMessages = lastMessages   // 🔥 EKLENDİ
            };

            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Index(UserEditDto userEditDto)
        {

            if (!ModelState.IsValid)
                return View(userEditDto);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("UserLogin", "Login");

            user.Name = userEditDto.Name;
            user.Surname = userEditDto.Surname;
            user.Email = userEditDto.Email;

            // Şifre güncelleme
            if (!string.IsNullOrEmpty(userEditDto.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher
                    .HashPassword(user, userEditDto.Password);
            }

            // Resim yükleme
            if (userEditDto.Image != null && userEditDto.Image.Length > 0)
            {
                var extension = Path.GetExtension(userEditDto.Image.FileName);
                var imageName = Guid.NewGuid() + extension;

                var saveLocation = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images",
                    imageName
                );

                using var stream = new FileStream(saveLocation, FileMode.Create);
                await userEditDto.Image.CopyToAsync(stream);

                user.ImageUrl = "/images/" + imageName;
            }

            // 🔥 UPDATE HER DURUMDA ÇALIŞIYOR
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(userEditDto);
        }
    }
}
