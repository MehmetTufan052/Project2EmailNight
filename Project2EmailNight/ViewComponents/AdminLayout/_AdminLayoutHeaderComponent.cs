using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2EmailNight.Context;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.ViewComponents.AdminLayout
{
    public class _AdminLayoutHeaderComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public _AdminLayoutHeaderComponent(UserManager<AppUser> userManager,EmailContext emailContext)
        {
            _userManager = userManager;
            _context = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return View(null);


            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            
            if (user == null)
                return View(null);

            var unreadMessages = await _context.Messages
              .Where(x => x.ReceiverEmail == user.Email
                       && !x.IsStatus
                       && !x.IsDeleted
                       && !x.IsDraft)
              .OrderByDescending(x => x.SendDate)
              .Take(5)
              .ToListAsync();
            var allUnreadCount = await _context.Messages
    .CountAsync(x => x.ReceiverEmail == user.Email
                  && !x.IsStatus
                  && !x.IsDeleted
                  && !x.IsDraft);

            var lastFiveMessages = await _context.Messages
                .Where(x => x.ReceiverEmail == user.Email
                         && !x.IsStatus
                         && !x.IsDeleted
                         && !x.IsDraft)
                .OrderByDescending(x => x.SendDate)
                .Take(5)
                .ToListAsync();

            var dto = new UserHeaderUIDto
            {
                FullName = $"{user.Name} {user.Surname}",
                ImageUrl = string.IsNullOrWhiteSpace(user.ImageUrl)
                ? "/assets/images/avatars/avatar-1.png"
                : user.ImageUrl,

                UnreadMessages = lastFiveMessages,
                UnreadCount = allUnreadCount

            };
            return View(dto);
        }
    }
}
