namespace WebApiProject.DTO
{
    public class AddLostItemCommentDTO
    {
        public string Description { set; get; }
        public DateTime CreatedAt { get; set; }

        public string UserId { set; get; }
        public int ItemLostID { set; get; }

    }
}
