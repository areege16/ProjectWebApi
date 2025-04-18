using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiProject.DTO;
using WebApiProject.Hubs;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemMsgsController : ControllerBase
    {

        private readonly ILostItemMsgRepo lostItemMsgRepository;
        private readonly IHubContext<HLostItemChatHub> LostItemChatHubContext;
        private readonly ILostItemCurrentUserRepo currentUserRepo;
        private readonly LostFoundContext context;

        public LostItemMsgsController(ILostItemMsgRepo lostItemMsgRepository, IHubContext<HLostItemChatHub> LostItemChatHubContext, ILostItemCurrentUserRepo currentUserRepo, LostFoundContext context)
        {
            this.lostItemMsgRepository = lostItemMsgRepository;
            this.LostItemChatHubContext = LostItemChatHubContext;
            this.currentUserRepo = currentUserRepo;
            this.context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await lostItemMsgRepository.GetUsers();
            return Ok(users);
        }


        [HttpPost("chats")]
        public async Task<IActionResult> chatAsync([FromBody] string selectedUserId)
        {
            var chat = await lostItemMsgRepository.GetMsgs(selectedUserId);
            return Ok(chat);
        }

        [HttpPost("SendMsg")]
        public async Task<IActionResult> SendMsgAsync(SendMsgDTO msgDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var nowDate = DateTime.Now;
                    var date = nowDate.ToShortDateString();
                    var time = nowDate.ToShortTimeString();

                    string senderID = await currentUserRepo.GetUserIdAsync();

                    var msgItem = new ChatLostItem()
                    {
                        Message = msgDTO.Msg,
                        Date = nowDate,
                        SenderId = senderID,
                        ReceiverId = msgDTO.reciverId,
                        ItemLostId = msgDTO.ItemLostId
                    };

                    context.chatLostItems.Add(msgItem);
                    context.SaveChanges();
                    List<string> users = new List<string>()
                    {
                        msgDTO.reciverId,senderID
                    };
                    await LostItemChatHubContext.Clients.Users(users).SendAsync("ReceieveMsg", msgDTO.Msg, date, time, senderID);
                    return Ok(new
                    {
                        status = "msg sent successfully",
                        sender = senderID,
                        receiver = msgDTO.reciverId,
                        msgContent = msgDTO.Msg,
                        msgTime = nowDate,
                        msgItemID = msgDTO.ItemLostId,
                    });
                }
                catch (Exception e)
                {

                    return BadRequest(e.InnerException.Message);
                }
            }
            return BadRequest(ModelState);

        }
    }
}
