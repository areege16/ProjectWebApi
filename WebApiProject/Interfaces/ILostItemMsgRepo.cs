using WebApiProject.DTO;

namespace WebApiProject.Interfaces
{
    public interface ILostItemMsgRepo
    {
        Task<List<LostItemLastMsgsUserDTO>> GetUsers();
        Task<LostItemChatDTO> GetMsgs(string SelectedUserId);
    }
}
