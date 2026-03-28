using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Webworks.Context;
using Webworks.DTOs;
using Webworks.DTOs.Repsonses;
using Webworks.Service;

namespace Webworks.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly TokenService tokenService;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext context, TokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var user = new IdentityUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email
            };
            var result = await userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return Unauthorized(new ErrorInfo("Invalid username or password"));
            }

            var result = await signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ErrorInfo("Invalid username or password"));
            }

            var tokenResult = await tokenService.GenerateToken(user);
            return Ok(new AuthResponse { 
                Token = tokenResult.Token, 
                Email = user.Email!, 
                Username = user.UserName!, 
                ExpiresAt = tokenResult.ExpiresAt
            });
        }
    }
}
