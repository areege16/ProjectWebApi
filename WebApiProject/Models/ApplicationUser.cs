using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("Sender")]
        public ICollection<ChatLostItem>? SentMsgs { get; set; }



        [InverseProperty("Receiver")]
        public ICollection<ChatLostItem>? ReceivedMsgs { get; set; }



        [InverseProperty(nameof(ChatFoundItem.Sender))]
        public virtual ICollection<ChatFoundItem>? SentMessages { get; set; }
        [InverseProperty(nameof(ChatFoundItem.Receiver))]
        public virtual ICollection<ChatFoundItem>? ReseivedMessages { get; set; }
    }
}
