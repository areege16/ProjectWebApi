using WebApiProject.Models;

namespace WebApiProject.Interfaces
{
    public interface IFoundItemRepository:IGenericRepository<ItemFound>
    {
       Task<List<ItemFound>> GetAllAsyncWithUserCommentsChats();

        Task<List<ItemFound>> GetAllByUserIdAsync(string userId);
    }
}
