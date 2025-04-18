using WebApiProject.enums;
using WebApiProject.Models;

namespace WebApiProject.Interfaces
{
    public interface ILostItemRepo : IGenericRepository<ItemLost>
    {
        List<ItemLost> GetItemsByUserID(string userId);
        List<ItemLost> Search(string keyword);
        List<ItemLost> GetItemsByCategory(Category Category);

    }
}
