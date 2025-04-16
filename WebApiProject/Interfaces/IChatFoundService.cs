using WebApiProject.DTO;

namespace WebApiProject.Interfaces
{
    public interface IChatFoundService
    {
        Task<IEnumerable<ChatFoundUserListDTO>> GetUsers();
        Task<ChatFoundDTO> GetChat(string selectedUserId);
    }
}
