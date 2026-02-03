using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Context;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext emailContext,UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CreateMessage(Message message)
        {
            message.SendDate = DateTime.Now;
            message.IsStatus = false;
            _emailContext.Messages.Add(message);
            _emailContext.SaveChanges();
            return RedirectToAction("Sendbox");
        }

        public async Task<IActionResult> Inbox()
        {
            var user=await _userManager.FindByNameAsync(User.Identity.Name);
            var messageList = _emailContext.Messages.Where(x => x.ReceiverEmail == user.Email).ToList();
            return View(messageList);
        }
    }
}
