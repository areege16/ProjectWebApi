using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class LostItemRepo : GenericRepository<ItemLost>, ILostItemRepo
    {
        private readonly LostFoundContext context;

        public LostItemRepo(LostFoundContext context) : base(context)
        {
            this.context = context;
        }


    }
}
