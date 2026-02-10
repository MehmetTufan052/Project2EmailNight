using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.ViewComponents.AdminLayout
{
    public class _AdminLayoutHeaderComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _AdminLayoutHeaderComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return View(null);


            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var dto = new UserHeaderUIDto
            {
                FullName = $"{user.Name} {user.Surname}",
                ImageUrl = string.IsNullOrWhiteSpace(user.ImageUrl)
                ? "/assets/images/avatars/avatar-1.png"
                : user.ImageUrl

            };
            return View(dto);
        }
    }
}
