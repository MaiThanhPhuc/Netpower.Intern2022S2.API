using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netpower.Intern2022S2.DTOs;
using Netpower.Intern2022S2.Services.Interfaces;
namespace Netpower.Intern2022S2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var result = _userServices.Get();
            return StatusCode(200,result);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            Guid userId;
            if (!Guid.TryParse(id, out userId))
            {
                return StatusCode(400,new ApiResponse(400, "User id can not empty!", null!));
            }
            var result = await _userServices.GetById(userId);
            return StatusCode(result.StatusCode, result);
        }

        // POST api/<UserController>
        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<UserDTO>> Post(UserDTO user)
        //{
        //    if (await _userServices.Post(user) == null)
        //    {
        //        return StatusCode(400, new ApiResponse(400, "User already exist", null!));
        //    }
        //    return StatusCode(200, new ApiResponse(200, "Success", user));
        //}

        // PUT api/<UserController>/5
        [HttpPut]
        [Authorize]

        public async Task<IActionResult> PutUser([FromForm] UserUpdateDTO user)
        {
            var result = await _userServices.PutUser(user);
            return StatusCode(result.StatusCode,result);
        }

    }
}
