using AutoMapper;
using TestProject.Data;
using TestProject.Models;
using TestProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public AccountController(UserManager<ApiUser> userManager,
            IMapper mapper,
            IAuthManager authManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            var pwd = _userManager.PasswordHasher.HashPassword(user, userDTO.Password);
            user.PasswordHash = pwd;
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user, userDTO.Roles);
            return Ok("Registration successful...");

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!await _authManager.ValidateUser(userDTO))
            {
                return Unauthorized();
            }

            return Accepted(new { Token = await _authManager.CreateToken() });

        }
    }
}