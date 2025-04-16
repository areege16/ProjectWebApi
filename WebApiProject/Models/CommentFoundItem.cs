using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Models
{
    public class CommentFoundItem
    {
        public int ID { get; set; }
        public string Description { set; get; }
        public DateTime CreatedAt { get; set; }


        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser? User { get; set; }


        [ForeignKey("ItemFound")]
        public int ItemFoundID { set; get; }
        public ItemFound? ItemFound { set; get; }
    }
}
