using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("/api/users")]

    public class UserController : ControllerBase {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository) {
            _userRepository = userRepository;
        }


        [HttpPost("")]
        public async Task<IActionResult> AddUser(UserToAdd userToAdd) {
            await _userRepository.AddUser(userToAdd);
            return Ok();
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login(LoginCredentials loginCredentials) {
            var res = await _userRepository.FindUser(loginCredentials);

            if (res == null) {
                return Unauthorized();
            }

            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim("Id", res!.Id.ToString())
            }, "Auth_cookie");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await Request.HttpContext.SignInAsync("Auth_cookie", claimsPrincipal, new AuthenticationProperties {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(12 * 60 * 60)
            });

            return Ok(new User {
                Id = res.Id,
            });
        }


        [HttpDelete("/api/logout")]
        public async Task<IActionResult> Logout() {
            await Request.HttpContext.SignOutAsync("Auth_cookie");

            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers() {
            var users = await _userRepository.GetUsers();

            return Ok(users);
        }
    }

}