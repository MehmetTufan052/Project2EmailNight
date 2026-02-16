namespace Project2EmailNight.Models
{
    public class MessageDetailsViewModel
    {
      
            public int MessageId { get; set; }
            public string SenderEmail { get; set; }
            public string ReceiverEmail { get; set; }
            public string Subject { get; set; }
            public string MessageDetail { get; set; }
            public DateTime SendDate { get; set; }
            public bool IsStatus { get; set; }
            public bool IsDraft { get; set; }
            public bool IsStarred { get; set; }
        
    }
}
