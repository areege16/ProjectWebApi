using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Models
{
    public class CommentLostItem
    {
        public int ID { get; set; }
        public string Description { set; get; }
        public DateTime CreatedAt { get; set; }


        [ForeignKey("User")]
        public string UserId { set; get; }
        public ApplicationUser User { get; set; }


        [ForeignKey("itemLost")]
        public int ItemLostID { set; get; }
        public ItemLost itemLost { set; get; }


       

    }
}
