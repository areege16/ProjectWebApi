using WebApiProject.Models;

namespace WebApiProject.Interfaces
{
    public interface IFoundCommentRepo: IGenericRepository<CommentFoundItem>
    {
        Task<CommentFoundItem> AddCommentsByItemFoundIdAsync(int itemFoundId);

    }
}
