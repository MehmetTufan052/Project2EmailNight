using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var dto = new UserEditDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
                    ? "/assets/images/avatars/avatar-1.png"
                    : user.ImageUrl
            };

            return View(dto);
        }
        

        [HttpPost]
        public async Task<IActionResult> Index(UserEditDto userEditDto)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = userEditDto.Name;
            user.Surname = userEditDto.Surname;
            user.Email = userEditDto.Email;

            if (!string.IsNullOrEmpty(userEditDto.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher
                    .HashPassword(user, userEditDto.Password);
            }

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

                // 🔥 EN KRİTİK SATIR
                user.ImageUrl = "/images/" + imageName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                
            }
            return View(userEditDto);
        }
    }
}
