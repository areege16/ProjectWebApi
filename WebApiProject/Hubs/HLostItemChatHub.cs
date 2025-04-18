using Microsoft.AspNetCore.SignalR;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Hubs
{

    public class HLostItemChatHub : Hub
    {
        private readonly LostFoundContext context;
        private readonly ILostItemCurrentUserRepo currentUserRepo;

        public HLostItemChatHub(LostFoundContext context, ILostItemCurrentUserRepo CurrentUserRepo)
        {
            this.context = context;
            currentUserRepo = CurrentUserRepo;

        }


        public async Task SendMsg(string reciverId, string Msg)
        {
            var nowDate = DateTime.Now;
            var date = nowDate.ToShortDateString();
            var time = nowDate.ToShortTimeString();

            string senderID = await currentUserRepo.GetUserIdAsync();

            var msgItem = new ChatLostItem()
            {
                Message = Msg,
                Date = nowDate,
                SenderId = senderID,
                ReceiverId = reciverId
            };

            context.chatLostItems.Add(msgItem);
            context.SaveChanges();
            List<string> users = new List<string>()
            {
                reciverId,senderID
            };
            await Clients.Users(users).SendAsync("ReceieveMsg", Msg, date, time, senderID);

        }
    }

}
