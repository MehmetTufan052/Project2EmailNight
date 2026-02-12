using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Project2EmailNight.Context;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;
using Project2EmailNight.Models;
using Microsoft.AspNetCore.Identity;

namespace Project2EmailNight.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EmailController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> SendEmail()
        {
            var user = await _userManager.GetUserAsync(User);

            var inboxMessages = _context.Messages
       .Where(x => x.ReceiverEmail == user.Email)
       .OrderByDescending(x => x.SendDate)
       .ToList();

            var model = new SendEmailPageViewModel()
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = inboxMessages
            };

            var unreadCount = inboxMessages.Count(x => x.IsStatus == false);
            ViewBag.UnreadCount = unreadCount;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailPageViewModel model)
        {
            var mailRequestDto = model.MailRequest;

            MimeMessage mimeMessage = new MimeMessage();


            MailboxAddress mailboxAddressFrom = new MailboxAddress("Identity Admin", "email");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", mailRequestDto.ReceiverEmail);
            mimeMessage.To.Add(mailboxAddressTo);

            mimeMessage.Subject = mailRequestDto.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailRequestDto.MessageDetail;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("email", "key");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);

            var user = await _userManager.GetUserAsync(User);

            Message message = new Message()
            {
                SenderEmail = user.Email,
                ReceiverEmail = mailRequestDto.ReceiverEmail,
                Subject = mailRequestDto.Subject,
                MessageDetail = mailRequestDto.MessageDetail,
                SendDate = DateTime.Now,
                IsStatus = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("SendEmail");

        }
    
    public async Task<IActionResult> MessageDetails(int id)
        {
            
            var user = await _userManager.GetUserAsync(User);

            var message = _context.Messages
                .FirstOrDefault(x => x.MessageId == id
                                  && x.ReceiverEmail == user.Email);

            if (message == null)
                return NotFound();

            // Okundu yap
            if (message.IsStatus == false)
            {
                message.IsStatus = true;
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

    }
}

