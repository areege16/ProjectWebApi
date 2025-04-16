using Microsoft.EntityFrameworkCore;
using WebApiProject.DTO;
using WebApiProject.Helpers;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Service
{
    public class ChatFoundService : IChatFoundService
    {
        public readonly LostFoundContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ChatFoundService(LostFoundContext context,ICurrentUserService currentUserService) 
        {
            this._context = context;
            this._currentUserService = currentUserService;
        }
        public async Task<ChatFoundDTO> GetChat(string selectedUserId)
        {
            var currentUserId = _currentUserService.UserId;

            var selectedUser = await _context.Users.FirstOrDefaultAsync(i=>i.Id==selectedUserId);
            var selectedUserName = "";
            if (selectedUser != null)
            {
                selectedUserName = selectedUser.UserName;
            }

            var chatDTO = new ChatFoundDTO()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectedUserId,
                ReceiverUserNsme = selectedUserName
            };

            var messages = await _context.chatFoundItems.Where(i=>(i.SenderId==currentUserId||i.SenderId==selectedUserId)&&(i.ReceiverId==selectedUserId||i.ReceiverId==currentUserId)).Select(i => new UserMessagesListDTO
            {
                Id=i.Id,
                Message = i.Message,
                Date=i.Date.ToShortDateString(),
                Time = i.Date.ToShortDateString(),
                IsCurrentUserSentMessage = i.SenderId==currentUserId,
            }).ToListAsync();

            chatDTO.Messages = messages;

            return chatDTO;
        }

        public async Task<IEnumerable<ChatFoundUserListDTO>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;

            var users = await _context.Users.Where(i => i.Id != currentUserId).Select(i => new ChatFoundUserListDTO()
            {
                Id = i.Id,
                UserName = i.UserName,
                LastMessage = _context.chatFoundItems.Where(m => (m.SenderId == currentUserId || m.SenderId == i.Id) && (m.ReceiverId == currentUserId || m.SenderId == i.Id)).OrderByDescending(m => m.Id).Select(m=>m.Message).FirstOrDefault()
            }).ToListAsync();
            return users;
        }
    }
}
