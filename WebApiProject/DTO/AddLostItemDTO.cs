using System.ComponentModel.DataAnnotations.Schema;
using WebApiProject.enums;
using WebApiProject.Models;

namespace WebApiProject.DTO
{
    public class AddLostItemDTO
    {
        public string name { set; get; }
        public string description { set; get; }
        public Category category { set; get; }
        public string Location { set; get; }
        public string? Image { set; get; }
        public ItemStatus Status { set; get; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateFound { get; set; }
        public string UserId { set; get; }

    }
}
