using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.DTO;
using WebApiProject.enums;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Repository;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemController : ControllerBase
    {
        private readonly ILostItemRepo lostItemRepo;
        private readonly ILostItemCurrentUserRepo currentUserRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public LostItemController(ILostItemRepo lostItemRepo, ILostItemCurrentUserRepo currentUserRepo, UserManager<ApplicationUser> userManager)
        {
            this.lostItemRepo = lostItemRepo;
            this.currentUserRepo = currentUserRepo;
            this.userManager = userManager;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddLostItem(AddLostItemDTO itemFromRequest)
        {
            ApplicationUser currentUser = await currentUserRepo.GetUser();
            if (ModelState.IsValid)
            {
                ItemLost itemLost = new ItemLost()
                {
                    name = itemFromRequest.name,
                    description = itemFromRequest.description,
                    category = itemFromRequest.category,
                    Location = itemFromRequest.Location,
                    Image = itemFromRequest.Image,
                    Status = itemFromRequest.Status,
                    CreatedAt = itemFromRequest.CreatedAt,
                    DateFound = itemFromRequest.DateFound,
                    UserId = currentUser.Id,
                };
                await lostItemRepo.InsertAsync(itemLost);
                await lostItemRepo.SaveAsync();
                return Ok(
                    new
                    {
                        msg = "item added successfully",
                        itemId = itemLost.ID
                    });

            }
            else
            {

                return BadRequest(ModelState);
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditLostItemAsync(EditLostItemDTO itemFromRequest)
        {
            ApplicationUser currentUser = await currentUserRepo.GetUser();
            if (ModelState.IsValid)
            {
                ItemLost itemLost = new ItemLost()
                {
                    ID = itemFromRequest.Id,
                    name = itemFromRequest.name,
                    description = itemFromRequest.description,
                    category = itemFromRequest.category,
                    Location = itemFromRequest.Location,
                    Image = itemFromRequest.Image,
                    Status = itemFromRequest.Status,
                    CreatedAt = itemFromRequest.CreatedAt,
                    DateFound = itemFromRequest.DateFound,
                    UserId = currentUser.Id,
                };
                lostItemRepo.Update(itemLost);
                await lostItemRepo.SaveAsync();
                return Ok(
                    new
                    {
                        msg = "item edited successfully",
                        itemId = itemLost.ID
                    });

            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetLostItemsByUserId([FromHeader] string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                ApplicationUser user = await currentUserRepo.GetUser();
                if (user.Id == userId)
                {
                    var Items = lostItemRepo.GetItemsByUserID(userId);
                    return Ok(new
                    {
                        items = Items
                    });

                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();

        }

        [HttpGet("searchLost")]
        public IActionResult SearchLostItemDescription(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                var items = lostItemRepo.Search(keyword);
                if (items.Count == 0)
                {
                    return Ok(new
                    {
                        FoundedItems = "No item founded"
                    });
                }
                return Ok(new
                {
                    FoundedItems = items
                });
            }
            else
            {

                return BadRequest("search word must not be empty");
            }
        }

        [HttpGet("GetByCategory")]
        public IActionResult GetLostItemsByCategory(Category Category)
        {

            var items = lostItemRepo.GetItemsByCategory(Category);
            return Ok(new
            {
                FoundedItems = items
            });
        }


    }
}
