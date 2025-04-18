using WebApiProject.enums;

namespace WebApiProject.DTO
{
    public class EditLostItemDTO
    {
        public int Id { get; set; }
        public string name { set; get; }
        public string description { set; get; }
        public Category category { set; get; }
        public string Location { set; get; }
        public string? Image { set; get; }
        public ItemStatus Status { set; get; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateFound { get; set; }
    }
}
