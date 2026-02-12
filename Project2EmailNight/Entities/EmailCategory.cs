namespace Project2EmailNight.Entities
{
    public class EmailCategory
    {
        public int EmailCategoryId { get; set; }
        public string Name { get; set; }

        public List<Message> Messages { get; set; }
    }
}
