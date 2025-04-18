using System.ComponentModel.DataAnnotations.Schema;
using WebApiProject.enums;
using WebApiProject.Models;

namespace WebApiProject.DTO
{
    public class ShowAllItemsDTO
    {
        public int ID { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public Category category { set; get; }
        public string Location { set; get; }
        public string Image { set; get; }
        public ItemStatus Status { set; get; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateFound { get; set; }

        public string UserName { set; get; }

        public ICollection<CommentFoundItem>? comments { get; set; }
        public ICollection<ChatFoundItem>? Chats { get; set; }

    }
}
