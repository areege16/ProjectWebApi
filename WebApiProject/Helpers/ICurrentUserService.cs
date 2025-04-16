using WebApiProject.Models;

namespace WebApiProject.Helpers
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Task<ApplicationUser> GetUser();
    }
}
