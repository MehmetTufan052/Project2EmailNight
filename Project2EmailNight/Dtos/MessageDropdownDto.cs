namespace Project2EmailNight.Dtos
{
    public class MessageDropdownDto
    {
        public int MessageId { get; set; }
        public string SenderMail { get; set; }
        public string Subject { get; set; }
        public DateTime SendDate { get; set; }
    }
}
