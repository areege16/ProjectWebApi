using System.ComponentModel.DataAnnotations.Schema;
using WebApiProject.enums;
using WebApiProject.Models;

namespace WebApiProject.DTO
{
    public class ItemInsertDTO
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public Category category { set; get; }
        public string Location { set; get; }
        public string?Image { set; get; }
        public ItemStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        //public string UserId { set; get; }

    }
}
