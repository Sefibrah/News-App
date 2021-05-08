using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers {
    [AllowAnonymous]
    public class AccountController : BaseApiController {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(ITokenService tokenService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper) {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto) {
            if (UserExists(registerDto.Email)) return BadRequest("Email already in use.");
            var user = _mapper.Map<AppUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            return Ok("Registered successfuly.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();
            return Ok(new UserDto {
                Username = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user)
            });
        }

        private bool UserExists(string email) {
            if (_userManager.Users.SingleOrDefault(u => u.Email == email) != null)
                return true;
            return false;
        }
    }
}