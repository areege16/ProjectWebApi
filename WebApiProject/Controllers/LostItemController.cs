using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.DTO;
using WebApiProject.Models;
using WebApiProject.Repository;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemController : ControllerBase
    {
        private readonly LostItemRepo lostItemRepo;

        public LostItemController(LostItemRepo lostItemRepo)
        {
            this.lostItemRepo = lostItemRepo;
        }
        [HttpPost]
        public async Task<IActionResult> AddLostItem(AddLostItemDTO itemFromRequest)
        {
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
                    UserId = itemFromRequest.UserId,
                };
                await lostItemRepo.InsertAsync(itemLost);
                await lostItemRepo.SaveAsync();

            }
            return BadRequest(ModelState);
        }
    }
}
