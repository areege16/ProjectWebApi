using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Models
{
    public class ChatFoundItem
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }



        [ForeignKey("ItemFound")]
        public int ItemFoundId { get; set; }
        public ItemFound ItemFound { get; set; }


        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }


        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
