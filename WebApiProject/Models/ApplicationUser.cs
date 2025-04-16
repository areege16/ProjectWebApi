using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApiProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("Sender")]
        public ICollection<ChatFoundItem>? SentMsgs { get; set; }



        [InverseProperty("Receiver")]
        public ICollection<ChatFoundItem>? ReceivedMsgs { get; set; }
    }
}
