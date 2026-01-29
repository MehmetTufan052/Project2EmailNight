using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Context;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _emailContext;

        public MessageController(EmailContext emailContext)
        {
            _emailContext = emailContext;
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
    }
}
