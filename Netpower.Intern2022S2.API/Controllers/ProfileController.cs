using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Services.Interfaces;


namespace Netpower.Intern2022S2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileServices _profileServices;

        public ProfileController(IProfileServices profileServices)
        {
            _profileServices = profileServices;
        }

        // GET: api/<ProfileController>
        [HttpGet]
        //[Authorize]

        public IActionResult Get()
        {
            var result =  _profileServices.GetAll();
            return Ok(new ApiResponse(200, "Success", result));
        }

        // GET api/<ProfileController>/5
        [HttpGet("{id}")]
        //[Authorize]

        public async Task<ActionResult<Profile>> Get(string id)
        {

            Guid userId;
            if (!Guid.TryParse(id, out userId))
            {
                return Ok(new ApiResponse(400, "User id can not empty!", null!));
            }

            var result = await _profileServices.GetById(userId);
            
            return Ok(result);
        }

        [HttpPut]
        //[Authorize]
        // PUT api/<ProfileController>/5
        public async Task<IActionResult> PutProfile([FromForm]ProfileDTO profile)
        {
            var result = await _profileServices.PutProfile(profile);
            return Ok(result);
        }

         // Upload image
        [HttpPost("/api/saveImage")]
        public async Task<IActionResult> SaveImage([FromForm] FileUpload fileObj)
        {
            var result = await _profileServices.SaveImage(fileObj);
            return Ok(result);
        }
    }
}
