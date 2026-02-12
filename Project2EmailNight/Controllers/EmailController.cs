using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Project2EmailNight.Context;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailContext _context;

        public EmailController(EmailContext context)
        {
            _context = context;
        }

        public IActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(MailRequestDto mailRequestDto)
        {
            MimeMessage mimeMessage=new MimeMessage();


            MailboxAddress mailboxAddressFrom = new MailboxAddress("Identity Admin", "tufaneser8@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", mailRequestDto.ReceiverEmail);
            mimeMessage.To.Add(mailboxAddressTo);

            mimeMessage.Subject = mailRequestDto.Subject;

            var bodyBuilder =new BodyBuilder();
            bodyBuilder.HtmlBody = mailRequestDto.MessageDetail;
            mimeMessage.Body=bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("tufaneser8@gmail.com", "rghp xhld nktr pxmb");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);


            Message message = new Message()
            {
                SenderEmail = User.Identity.Name,
                ReceiverEmail = mailRequestDto.ReceiverEmail,
                Subject = mailRequestDto.Subject,
                MessageDetail = mailRequestDto.MessageDetail,
                SendDate = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("SendEmail");
            
        }
    }
}
