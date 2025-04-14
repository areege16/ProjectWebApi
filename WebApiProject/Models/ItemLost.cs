using System.ComponentModel.DataAnnotations.Schema;
using WebApiProject.enums;

namespace WebApiProject.Models
{
    public class ItemLost
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


        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser User { get; set; }

        public ICollection<CommentLostItem>? comments { get; set; }
        public ICollection<ChatLostItem>? Chats { get; set; }

    }
}
