using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiProject.Helpers;
using WebApiProject.Hubs;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatFoundController : ControllerBase
    {
        private readonly IChatFoundService chatFoundService;
        private readonly ICurrentUserService currentUserService;
        private readonly LostFoundContext context;
        private readonly IHubContext<ChatFoundHub> hubContext;
        public ChatFoundController(IChatFoundService chatFoundService, ICurrentUserService currentUserService, LostFoundContext context,IHubContext<ChatFoundHub> hubContext)
        {
            this.chatFoundService = chatFoundService;
            this.currentUserService = currentUserService;
            this.context = context;
            this.hubContext = hubContext;
        }
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var users = await chatFoundService.GetUsers();
            return Ok(users);
        }
        [HttpGet("chat/{selectUserId}")]
        public async Task<IActionResult> Chat(string selectUserId)
        {
            var chatDTO = await chatFoundService.GetChat(selectUserId);
            return Ok(chatDTO);
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage(string receiverId, string message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var NowDate = DateTime.Now;
                    var date = NowDate.ToShortDateString;
                    var time = NowDate.ToShortTimeString;

                    string senderId = currentUserService.UserId;
                    var messageToAdd = new ChatFoundItem()
                    {
                        Message = message,
                        SenderId = senderId,
                        Date = NowDate,
                        ReceiverId = receiverId
                    };

                    await context.AddAsync(messageToAdd);
                    await context.SaveChangesAsync();

                    List<string> users = new List<string>()
                    {
                    receiverId,senderId
                    };

                    await hubContext.Clients.Users(users).SendAsync("ReceiveMessage", message, date, time, senderId);

                    return Ok(messageToAdd);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException.Message);
                }

            }
            return BadRequest(ModelState);
        }
    }
}
