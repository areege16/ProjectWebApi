namespace WebApiProject.DTO
{
    public class AddFoundItemCommentDTO
    {
        public string Description { set; get; }
        public DateTime CreatedAt { get; set; }
        public int ItemFoundID { set; get; }
    }
}
