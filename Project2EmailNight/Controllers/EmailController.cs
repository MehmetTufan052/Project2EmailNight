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
       .Where(x => x.ReceiverEmail == user.Email
         && !x.IsDraft
         && !x.IsDeleted)
       .OrderByDescending(x => x.SendDate)
       .ToList();

            var model = new SendEmailPageViewModel()
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = inboxMessages
            };

            var unreadCount = inboxMessages.Count(x => x.IsStatus == false);
            ViewBag.UnreadCount = unreadCount;


            ViewBag.InboxCount = await _context.Messages
                .CountAsync(x => x.ReceiverEmail == user.Email
                              && !x.IsDeleted
                              && !x.IsDraft);

            ViewBag.DraftCount = await _context.Messages
                .CountAsync(x => x.ReceiverEmail == user.Email
                              && x.IsDraft
                              && !x.IsDeleted);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailPageViewModel model, string? actionType)
        {
            actionType ??= "send"; // güvenlik

            var user = await _userManager.GetUserAsync(User);
            var mailRequestDto = model.MailRequest;

            // SADECE gönder butonuna basıldıysa SMTP çalışır
            if (actionType == "send")
            {
                MimeMessage mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress("Identity Admin", "email"));
                mimeMessage.To.Add(new MailboxAddress("User", mailRequestDto.ReceiverEmail));
                mimeMessage.Subject = mailRequestDto.Subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = mailRequestDto.MessageDetail;
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, false);
                    smtpClient.Authenticate("email", "key");
                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);
                }
            }

            // Taslak mı gönderim mi?
            Message message = new Message()
            {
                SenderEmail = user.Email,
                ReceiverEmail = mailRequestDto.ReceiverEmail,
                Subject = mailRequestDto.Subject,
                MessageDetail = mailRequestDto.MessageDetail,
                SendDate = DateTime.Now,
                IsStatus = false,
                IsDraft = actionType == "draft"
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(actionType == "draft" ? "Drafts" : "SendEmail");
        }





        public async Task<IActionResult> MessageDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id
                                       && x.ReceiverEmail == user.Email);

            if (message == null)
                return NotFound();

            if (!message.IsStatus)
            {
                message.IsStatus = true;
                await _context.SaveChangesAsync();
            }

            var viewModel = new MessageDetailsViewModel
            {
                MessageId = message.MessageId,
                SenderEmail = message.SenderEmail,
                ReceiverEmail = message.ReceiverEmail,
                Subject = message.Subject,
                MessageDetail = message.MessageDetail,
                SendDate = message.SendDate,
                IsStatus = message.IsStatus

                
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStar(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id
                                       && x.ReceiverEmail == user.Email);

            if (message == null)
                return Json(new { success = false });

            message.IsStarred = !message.IsStarred;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isStarred = message.IsStarred });
        }

        public async Task<IActionResult> Starred()
        {
            var user = await _userManager.GetUserAsync(User);

            var messages = await _context.Messages
                .Where(x => x.IsStarred
         && x.ReceiverEmail == user.Email
         && !x.IsDeleted)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            var model = new SendEmailPageViewModel
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = messages
            };

            return View("SendEmail", model);
        }

        public async Task<IActionResult> Sent()
        {
            var user = await _userManager.GetUserAsync(User);

            var sentMessages = await _context.Messages
                .Where(x => x.SenderEmail == user.Email
         && !x.IsDraft
         && !x.IsDeleted)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            var model = new SendEmailPageViewModel
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = sentMessages
            };

            return View("SendEmail", model);
        }
        public async Task<IActionResult> Drafts()
        {
            var user = await _userManager.GetUserAsync(User);

            var draftMessages = await _context.Messages
                .Where(x => x.IsDraft
         && x.SenderEmail == user.Email
         && !x.IsDeleted)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            var model = new SendEmailPageViewModel
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = draftMessages
            };

            ViewBag.PageTitle = "Taslaklar";

            return View("SendEmail", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id &&
                                         (x.ReceiverEmail == user.Email
                                          || x.SenderEmail == user.Email));

            if (message == null)
                return Json(new { success = false });

            message.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public async Task<IActionResult> Trash()
        {
            var user = await _userManager.GetUserAsync(User);

            var deletedMessages = await _context.Messages
                .Where(x => x.IsDeleted &&
                           (x.ReceiverEmail == user.Email
                            || x.SenderEmail == user.Email))
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            var model = new SendEmailPageViewModel
            {
                MailRequest = new MailRequestDto(),
                InboxMessages = deletedMessages
            };

            ViewBag.PageTitle = "Çöp Kutusu";

            return View("SendEmail", model);
        }
        [HttpPost]
        public async Task<IActionResult> BulkDelete([FromBody] List<int> ids)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ids == null || !ids.Any())
                return Json(new { success = false });

            var messages = await _context.Messages
                .Where(x => ids.Contains(x.MessageId) &&
                       (x.ReceiverEmail == user.Email
                        || x.SenderEmail == user.Email))
                .ToListAsync();

            foreach (var message in messages)
            {
                message.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> SetCategory(int id, int categoryId)
        {
            var user = await _userManager.GetUserAsync(User);

            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id &&
                                         (x.ReceiverEmail == user.Email
                                          || x.SenderEmail == user.Email));

            if (message == null)
                return Json(new { success = false });

            message.EmailCategoryId = categoryId;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }





    }

}


