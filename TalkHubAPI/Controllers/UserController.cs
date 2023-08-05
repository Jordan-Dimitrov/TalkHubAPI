using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Dto;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _Mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _UserRepository = userRepository;
            _Mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _Mapper.Map<List<UserDto>>(_UserRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

    }
}
