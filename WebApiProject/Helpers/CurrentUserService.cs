using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using WebApiProject.Models;

namespace WebApiProject.Helpers
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LostFoundContext _context;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor,LostFoundContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public string UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
                if (string.IsNullOrEmpty(id))
                {
                    id = _httpContextAccessor.HttpContext?.User.Claims
                     .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                return id;
            }
        }

        public async Task<ApplicationUser> GetUser()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            return user;
        }
    }
}
