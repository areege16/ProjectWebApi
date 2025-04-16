using Microsoft.AspNetCore.Identity;
using WebApiProject.DTO;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class LostItemMsgRepo : ILostItemMsgRepo
    {
        private readonly LostFoundContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILostItemCurrentUserRepo currentUserRepo;

        public LostItemMsgRepo(LostFoundContext context, UserManager<ApplicationUser> userManager, ILostItemCurrentUserRepo currentUserRepo)
        {
            this.context = context;
            this.userManager = userManager;
            this.currentUserRepo = currentUserRepo;
        }
        public async Task<LostItemChatDTO> GetMsgs(string SelectedUserId)
        {

            string CurrentUSerId = await currentUserRepo.GetUserIdAsync();
            ApplicationUser SelectedUser = await userManager.FindByIdAsync(SelectedUserId);
            string SelectedUserName = "";

            if (SelectedUser != null)
            {
                SelectedUserName = SelectedUser.UserName;
            }

            LostItemChatDTO chat = new LostItemChatDTO()
            {
                CurrentUserId = CurrentUSerId,
                ReceiverId = SelectedUserId,
                ReceiverUserName = SelectedUserName
            };

            var msgs = context.chatLostItems
                .Where(m => (m.SenderId == CurrentUSerId || m.SenderId == SelectedUserId) && (m.ReceiverId == CurrentUSerId))
                .Select(m => new LostItemMsgsInsideChatDTO()
                {
                    Id = m.Id,
                    Message = m.Message,
                    Date = m.Date.ToShortDateString(),
                    Time = m.Date.ToShortTimeString(),
                    IsCurrentUserSentMsg = m.SenderId == CurrentUSerId,
                }).ToList();

            chat.Msgs = msgs;

            return chat;
        }

        public async Task<List<LostItemLastMsgsUserDTO>> GetUsers()
        {
            string CurrentUSerId = await currentUserRepo.GetUserIdAsync();
            var users = context.Users.Where(u => u.Id != CurrentUSerId).Select(u => new LostItemLastMsgsUserDTO()
            {
                Id = u.Id,
                UserName = u.UserName,
                LastMsg = context.chatLostItems
                .Where(m => (m.SenderId == CurrentUSerId || m.SenderId == u.Id) && (m.ReceiverId == CurrentUSerId || m.ReceiverId == u.Id))
                .OrderByDescending(m => m.Id)
                .Select(m => m.Message)
                .FirstOrDefault()

            }).ToList();
            return users;
        }
    }
}
