using WebApiProject.Models;

namespace WebApiProject.DTO
{
    public class ChatFoundDTO
    {
        public ChatFoundDTO()
        {
            Messages = new List<UserMessagesListDTO>();
        }
        public string CurrentUserId {  get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUserNsme { get; set; }
        public IEnumerable<UserMessagesListDTO> Messages { get; set; }
    }
}
