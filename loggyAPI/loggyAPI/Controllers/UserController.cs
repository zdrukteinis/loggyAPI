using System.Linq;
using AutoMapper;
using loggyAPI.Attributes;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Entities.Enums;
using loggyAPI.Dtos;
using loggyAPI.Helpers;
using loggyAPI.Services;
using loggyAPI.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace loggyAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IOptions<AppSettings> appSettings, IMapper mapper, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password, _appSettings.Secret);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Role(Role.Admin)]
        [HttpPost("delete")]
        public IActionResult Delete([FromBody] UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _userService.Delete(user);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("update")]
        public IActionResult Update([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                var updateUser = _userService.Update(user, userDto.Password);
                return Ok(_mapper.Map<UserDto>(updateUser));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll().Select(x => _mapper.Map<UserDto>(x));
            return Ok(users);
        }
    }
}