using WebApiProject.enums;

namespace WebApiProject.DTO
{
    public class ItemUpdateDTO
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public Category category { set; get; }
        public string Location { set; get; }
        public string? Image { set; get; }
        public ItemStatus Status { get; set; }
        public DateTime DateFound { get; set; } 

    }
}
