using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    //if (identityResult.Succeeded)
                    //{
                    //    //return Ok("User has been registered successfully");
                    //    return Ok(identityResult);
                    //}
                }

                return Ok(identityResult);
            }

            return BadRequest("Something went wrong");
        }

        // [HttpPost]
        // [Route("Login")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        // {
        //     var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
        //
        //     if (user == null)
        //     {
        //         return BadRequest("Invalid login credentials");
        //     }
        //
        //     var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        //
        //     if (checkPasswordResult == false)
        //     {
        //         return BadRequest("Invalid login credentials");
        //     }
        //
        //     // Get Roles for this user
        //     var roles = await userManager.GetRolesAsync(user);
        //
        //     if (roles == null)
        //     {
        //         return BadRequest("User has no associated roles. Cannot proceed without roles");
        //     }
        //
        //     // Create Token
        //     var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
        //
        //     var response = new LoginResponseDto
        //     {
        //         JwtToken = jwtToken
        //     };
        //
        //     return Ok(response);
        // }
        
        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create Token

                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }
    }
}
