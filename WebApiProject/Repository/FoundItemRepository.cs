using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class FoundItemRepository:GenericRepository<ItemFound>, IFoundItemRepository
    {
        private readonly LostFoundContext context;

        public FoundItemRepository(LostFoundContext context ):base(context)
        {
            this.context = context;
        }

        public async Task<List<ItemFound>> GetAllAsyncWithUserCommentsChats()
        {
            return await context.itemsFound
                 .Include(f => f.User)
                 .Include(f => f.comments)
                 .Include(f => f.Chats)
                 .ToListAsync();
        }

        public async Task<List<ItemFound>> GetAllByUserIdAsync(string userId)
        {
            return await context.itemsFound
                .Where(f => f.UserId == userId)
                .Include(f => f.User)
                .Include(f => f.comments)
                .Include(f => f.Chats)
                .ToListAsync();
        }


    }
}
