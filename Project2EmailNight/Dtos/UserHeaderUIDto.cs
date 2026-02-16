using Project2EmailNight.Entities;

namespace Project2EmailNight.Dtos
{
    public class UserHeaderUIDto
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public List<Message> UnreadMessages { get; set; }
        public int UnreadCount { get; set; }

        public List<MessageDropdownDto> LastMessages { get; set; }
    }
}
