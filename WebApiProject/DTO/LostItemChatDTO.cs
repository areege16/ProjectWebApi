namespace WebApiProject.DTO
{
    public class LostItemChatDTO
    {
        public LostItemChatDTO()
        {
            Msgs = new List<LostItemMsgsInsideChatDTO>();
        }
        public string CurrentUserId { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
        public List<LostItemMsgsInsideChatDTO> Msgs { get; set; }
    }
}

