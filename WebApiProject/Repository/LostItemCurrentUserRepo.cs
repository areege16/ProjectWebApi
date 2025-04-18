using Microsoft.AspNetCore.Identity;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class LostItemCurrentUserRepo : ILostItemCurrentUserRepo
    {
        private readonly LostFoundContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LostItemCurrentUserRepo(LostFoundContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetUserIdAsync()
        {
            //string userName = httpContext.HttpContext.User.Identity.Name;

            //var userName = httpContextAccessor.HttpContext.User.Identities.
            //userManager.find
            var userName = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                ApplicationUser user = await userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    return user.Id;
                }
            }
            return "Not Found";
        }

        public async Task<ApplicationUser> GetUser()
        {
            return await userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity?.Name);
        }
    }
}
