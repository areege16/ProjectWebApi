using Microsoft.AspNetCore.SignalR;
using WebApiProject.Helpers;
using WebApiProject.Models;

namespace WebApiProject.Hubs
{
    public class ChatFoundHub : Hub
    {
        private readonly LostFoundContext context;
        private readonly ICurrentUserService currentUserService;

        public ChatFoundHub(LostFoundContext lostFoundContext,ICurrentUserService currentUserService)
        {
            this.context = lostFoundContext;
            this.currentUserService = currentUserService;
        }

        
    }
}
