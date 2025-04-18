namespace WebApiProject.DTO
{
    public class LostItemMsgsInsideChatDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsCurrentUserSentMsg { get; set; }
    }
}
