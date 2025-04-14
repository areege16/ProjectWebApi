using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiProject.Models
{
    public class LostFoundContext:IdentityDbContext<ApplicationUser>
    {
        public LostFoundContext(DbContextOptions<LostFoundContext> options):base(options)
        {
        }

        public DbSet<ItemLost> itemsLost { get; set; }
        public DbSet<ItemFound> itemsFound { get; set; }
        public DbSet<CommentFoundItem> commentFoundItems { get; set; }
        public DbSet<CommentLostItem>  commentLostItems { get; set; }
        public DbSet<ChatFoundItem> chatFoundItems { get; set; }
        public DbSet<ChatLostItem> chatLostItems { get; set; }
    }
}
