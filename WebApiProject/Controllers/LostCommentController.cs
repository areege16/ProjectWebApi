using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.DTO;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Repository;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostCommentController : ControllerBase
    {
        private readonly ILostCommentRepo lostCommentRepo;
        private readonly ILostItemCurrentUserRepo currentUserRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public LostCommentController(ILostCommentRepo lostCommentRepo, ILostItemCurrentUserRepo currentUserRepo, UserManager<ApplicationUser> userManager)
        {
            this.lostCommentRepo = lostCommentRepo;
            this.currentUserRepo = currentUserRepo;
            this.userManager = userManager;
        }


        [HttpPost("Comment")]
        public async Task<IActionResult> AddLostComment(AddLostItemCommentDTO commentFromRequest)
        {
            if (ModelState.IsValid)
            {
                CommentLostItem comment = new CommentLostItem()
                {
                    Description = commentFromRequest.Description,
                    CreatedAt = commentFromRequest.CreatedAt,
                    ItemLostID = commentFromRequest.ItemLostID,
                    UserId = commentFromRequest.UserId,

                };
                try
                {

                    await lostCommentRepo.InsertAsync(comment);
                    await lostCommentRepo.SaveAsync();
                    return Ok(new
                    {
                        msg = "comment inserted "
                    });
                }
                catch (Exception e)
                {

                    return BadRequest(e.InnerException.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}
