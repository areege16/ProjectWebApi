using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiProject.DTO;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoundItemController : ControllerBase
    {
        #region Constructor and inject
        private readonly IFoundItemRepository foundItemRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public FoundItemController(IFoundItemRepository foundItemRepository, UserManager<ApplicationUser> userManager)
        {
            this.foundItemRepository = foundItemRepository;
            this.userManager = userManager;
        } 
        #endregion

        #region Add new Found Item
        [HttpPost("insert")] //api/FoundItem/insert 
        public async Task<IActionResult> insert(ItemInsertDTO itemFound)
        {
            GeneralResponse generalResponse = new GeneralResponse();
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                try
                {
                    var item = new ItemFound
                    {
                        Name = itemFound.Name,
                        Description = itemFound.Description,
                        category = itemFound.category,
                        Location = itemFound.Location,
                        Image = itemFound.Image,
                        Status = itemFound.Status,
                        CreatedAt = DateTime.Now,
                        UserId = user.Id
                    };
                    await foundItemRepository.InsertAsync(item);
                    await foundItemRepository.SaveAsync();
                    generalResponse.IsPass = true;
                    generalResponse.Data = itemFound;
                    //return CreatedAtAction("GetById", new { id = item.ID }, generalResponse);

                    return Ok(generalResponse);
                    //return CreatedAtAction("GetById", new { id = itemFound.ID }, itemFound);
                }
                catch (Exception ex)
                {
                    generalResponse.IsPass = false;
                    generalResponse.Data = ex.InnerException?.Message ?? ex.Message;
                    return StatusCode(500, generalResponse);

                    //ModelState.AddModelError("", ex.InnerException?.Message??ex.Message);
                }
            }
            return BadRequest(generalResponse);
        }

        #endregion

        #region Update 
        [Authorize]
        [HttpPut("{id:int}")] // api//FoundItem/{id}
        public async Task<IActionResult> update(int id , ItemUpdateDTO itemFoundFromReq)
        {
            ItemFound itemFoundFromDB =await foundItemRepository.GetByIdAsync(id);
            GeneralResponse generalResponse = new GeneralResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return Unauthorized();
                    }
                    if (itemFoundFromDB != null)
                    {
                        if (itemFoundFromDB.UserId != user.Id)
                        {
                            generalResponse.IsPass = false;
                            generalResponse.Data = "You are not authorized to update this item.";
                            return Unauthorized(); 
                        }
                        itemFoundFromDB.Name = itemFoundFromReq.Name;
                        itemFoundFromDB.Description = itemFoundFromReq.Description;
                        itemFoundFromDB.category = itemFoundFromReq.category;
                        itemFoundFromDB.Location = itemFoundFromReq.Location;
                        itemFoundFromDB.Image = itemFoundFromReq.Image;
                        itemFoundFromDB.Status = itemFoundFromReq.Status;
                        itemFoundFromDB.DateFound = itemFoundFromReq.DateFound;

                        await foundItemRepository.SaveAsync();
                        generalResponse.IsPass = true;
                        generalResponse.Data = itemFoundFromReq;
                        return Ok(generalResponse);
                    }
                    generalResponse.IsPass = false;
                    generalResponse.Data = $"item with ID {id} Not Found";
                    return NotFound(generalResponse);
                }
                generalResponse.IsPass = false;
                generalResponse.Data = "Invalid model state.";
                return BadRequest(generalResponse);
            }
            catch (Exception ex)
            {
                generalResponse.IsPass = false;
                generalResponse.Data = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, generalResponse);
            }
        }

        #endregion

        #region Delete
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItemFound(int id)
        {
            ItemFound itemFound =await foundItemRepository.GetByIdAsync(id);
            GeneralResponse generalResponse = new GeneralResponse();

            if (itemFound == null)
            {
                generalResponse.IsPass = false;
                generalResponse.Data = $"Item With this ID {id} Not found";
                return NotFound(generalResponse);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (itemFound.UserId != user.Id)
            {
                generalResponse.IsPass = false;
                generalResponse.Data = "You are not authorized to delete this item.";
                return Unauthorized();
            }
            try
            {
                await foundItemRepository.DeleteAsync(id);
                await foundItemRepository.SaveAsync();

                generalResponse.IsPass = true;
                generalResponse.Data = $"Delete ID {id} Done Sussesfully";

                return Ok(generalResponse);
            }
            catch(Exception ex)
            {
                generalResponse.IsPass = false;
                generalResponse.Data = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, generalResponse);
            }
        }

        #endregion

        #region Get All FoundItems
        [HttpGet]
        public async Task<IActionResult> ShowAllFoundItems()
        {
            var FoundItems =await foundItemRepository.GetAllAsyncWithUserCommentsChats();

            var result = FoundItems.
                Select(item => new ShowAllItemsDTO
            {
                ID = item.ID,
                name = item.Name,
                description = item.Description,
                category = item.category,
                Location = item.Location,
                Image = item.Image,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                DateFound = item.DateFound,
                UserName = item.User.UserName,
                comments = item.comments,
                Chats = item.Chats
            }).ToList();

            return Ok(result);
        }
        #endregion

        #region GetItemFoundByUserID

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetAllFoundItemsByUserId()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var items = await foundItemRepository.GetAllByUserIdAsync(user.Id);

            var result = items.Select(item => new ShowAllItemsDTO
            {
                ID = item.ID,
                name = item.Name,
                description = item.Description,
                category = item.category,
                Location = item.Location,
                Image = item.Image,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                DateFound = item.DateFound,
                UserName = item.User.UserName,
                comments = item.comments,
                Chats = item.Chats
            }).ToList();

            return Ok(result);
        } 
        #endregion


    }
}
