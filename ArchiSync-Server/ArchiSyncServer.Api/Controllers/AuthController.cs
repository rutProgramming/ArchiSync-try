using AutoMapper;
using ArchiSyncServer.Api.Models;
using ArchiSyncServer.API.Models;
using ArchiSyncServer.core;
using ArchiSyncServer.core.Entities;
using ArchiSyncServer.core.Iservices;
using ArchiSyncServer.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArchiSyncServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRolesService _userRolesService;
        public AuthController(IConfiguration configuration, IUserService userService,AuthService authService,IMapper mapper,IUserRolesService userRolesService)
        {
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
            _userRolesService = userRolesService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var userRole = await _userService.Authenticate(model.UserName,model.Password);
            var roleName = userRole.Role.RoleName;
            if ( roleName== "Admin")
            {
                var token = _authService.GenerateJwtToken(model.UserName, new[] { "Admin" });
                return Ok(new { Token = token ,User=userRole.User ,RoleName= "Admin" });
            }
            else if (roleName == "editor")
            {
                var token = _authService.GenerateJwtToken(model.UserName, new[] { "Editor" });
                return Ok(new { Token = token, User = userRole.User, RoleName = "Editor" });
            }
            else if (roleName == "viewer" )
            {
                var token = _authService.GenerateJwtToken(model.UserName, new[] { "Viewer" });
                return Ok(new { Token = token, User = userRole.User, RoleName = "Viewer" });
            }

            return Unauthorized();
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var userDto = _mapper.Map<UserDTO>(model);
                var createdUser = await _userService.CreateUserAsync(userDto, model.RoleName);
                var token = _authService.GenerateJwtToken(model.UserName, new[] { model.RoleName });
                return Ok(new { Token = token ,UserId=createdUser.UserId});
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}



