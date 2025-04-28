using Microsoft.EntityFrameworkCore;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class FoundCommentRepo : GenericRepository<CommentFoundItem>, IFoundCommentRepo
    {
        private readonly LostFoundContext context;

        public FoundCommentRepo(LostFoundContext context):base(context)
        {
            this.context = context;
        }

        public async Task<CommentFoundItem> AddCommentsByItemFoundIdAsync(int itemFoundId)
        {
            return await context.commentFoundItems
                .Where(c => c.ItemFoundID == itemFoundId)
                .Include(c => c.User)
                .Include(c=>c.ItemFound)
                .FirstOrDefaultAsync();
        }
    }

}
