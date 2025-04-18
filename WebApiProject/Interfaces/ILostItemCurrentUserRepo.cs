using WebApiProject.Models;

namespace WebApiProject.Interfaces
{
    public interface ILostItemCurrentUserRepo
    {
        Task<string> GetUserIdAsync();
        Task<ApplicationUser> GetUser();
    }
}
