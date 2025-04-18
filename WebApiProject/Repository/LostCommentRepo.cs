using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class LostCommentRepo : GenericRepository<CommentLostItem>, ILostCommentRepo
    {
        private readonly LostFoundContext context;

        public LostCommentRepo(LostFoundContext context) : base(context)
        {
            this.context = context;
        }
    }
}
