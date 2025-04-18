using System.ComponentModel.DataAnnotations.Schema;
using WebApiProject.enums;

namespace WebApiProject.Models
{
    public class ItemFound
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public Category category { set; get; }
        public string Location { set; get; }
        public string Image { set; get; }
        public ItemStatus Status { set; get; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateFound { get; set; }


        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }


        public ICollection<CommentFoundItem>? comments { get; set; }
        public ICollection<ChatFoundItem>? Chats { get; set; }

    }
}
