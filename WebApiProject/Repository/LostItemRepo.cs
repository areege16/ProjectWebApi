using WebApiProject.enums;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace WebApiProject.Repository
{
    public class LostItemRepo : GenericRepository<ItemLost>, ILostItemRepo
    {
        private readonly LostFoundContext context;

        public LostItemRepo(LostFoundContext context) : base(context)
        {
            this.context = context;
        }

        public List<ItemLost> GetItemsByUserID(string userId)
        {
            return context.itemsLost.Where(i => i.UserId.Contains(userId)).ToList();
        }

        public List<ItemLost> Search(string keyword)
        {
            return context.itemsLost.Where(i => i.description.Contains(keyword)).ToList();
        }

        public List<ItemLost> GetItemsByCategory(Category Category)
        {
            return context.itemsLost.Where(i => i.category == Category).ToList();

        }


    }
}
