using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.DTO;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoundCommentController : ControllerBase
    {
        #region Constructor and inject
        private readonly IFoundCommentRepo foundCommentRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public FoundCommentController(IFoundCommentRepo foundCommentRepo, UserManager<ApplicationUser> userManager)
        {
            this.foundCommentRepo = foundCommentRepo;
            this.userManager = userManager;
        }
        #endregion

        #region Add new Found Item Comment

        [HttpPost("insert")]
        [Authorize]
        public async Task<IActionResult> Insert(AddFoundItemCommentDTO foundItemComment)
        {
            GeneralResponse generalResponse = new GeneralResponse();
            if (ModelState.IsValid)
            {
                var user =await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                try
                {
                    var NewComment = new CommentFoundItem
                    {
                        Description = foundItemComment.Description,
                        CreatedAt = DateTime.UtcNow,
                        UserId = user.Id,
                        ItemFoundID = foundItemComment.ItemFoundID
                    };
                    await foundCommentRepo.InsertAsync(NewComment);
                    await foundCommentRepo.SaveAsync();

                    var responseDTO = new AddFoundItemCommentDTO
                    {
                        Description = NewComment.Description,
                        CreatedAt = NewComment.CreatedAt,
                        ItemFoundID = NewComment.ItemFoundID
                    };

                    generalResponse.IsPass = true;
                    generalResponse.Data = foundItemComment;
                    return Ok(generalResponse);
                }
                catch (Exception ex)
                {
                    generalResponse.IsPass = false;
                    generalResponse.Data = ex.InnerException?.Message ?? ex.Message;
                    return StatusCode(500, generalResponse);
                }
            }
            return BadRequest(generalResponse);
        }

        #endregion
    }
}
