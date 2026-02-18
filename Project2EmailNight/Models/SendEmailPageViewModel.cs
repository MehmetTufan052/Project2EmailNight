using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Models
{
    public class SendEmailPageViewModel
    {
        public MailRequestDto MailRequest { get; set; }

        public List<Message> InboxMessages { get; set; }

        public List<EmailCategory> Categories { get; set; }
    }
}
