using Core.Hesh;
using Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User : Controller
    {
        private readonly IUser _userService;
        private readonly ILogger<User> _logger;
        private readonly IJWT _jwt;

        public User(IUser userService, ILogger<User> logger, IJWT jwt) 
        {
            _userService = userService;
            _logger = logger;
            _jwt = jwt;
        }

        [HttpGet("UpdateToken")]
        public async Task<IActionResult> UpdateAssentsToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh_token"];

                if(string.IsNullOrEmpty(refreshToken)) return BadRequest(new { error = "unauthorized" });

                string userId = _jwt.DecodeToken(refreshToken);

                if(string.IsNullOrEmpty(userId)) return BadRequest(new { error = "unauthorized" });

                var user = await _userService.GetUserById(userId);

                if (user == null) return BadRequest(new { error = "does not have such user" });

                bool isHave = await _userService.isHaveToken(userId, refreshToken);

                if (!isHave)
                {
                    return BadRequest(new { error = "unauthorized" });
                }
                else if(!_jwt.ValidateToken(refreshToken))
                {
                    _userService.DeleteRefreshToken(userId, refreshToken);
                    return BadRequest(new { error = "Invalid refresh token." });
                }

                var assetsToken = _jwt.GenerateToken(userId, user.Email, TokenType.Access);

                return Ok(new { Token = assetsToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the access token.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }


        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh_token"];

                if (string.IsNullOrEmpty(refreshToken)) return BadRequest(new { error = "unauthorized" });

                string userId = _jwt.DecodeToken(refreshToken);

                if (string.IsNullOrEmpty(userId)) return BadRequest(new { error = "unauthorized" });

                var user = await _userService.GetUserById(userId);

                if (user == null) return BadRequest(new { error = "does not have such user" });

                bool isHave = await _userService.isHaveToken(userId, refreshToken);

                if (!isHave)
                {
                    return BadRequest(new { error = "unauthorized" });
                }

                _userService.DeleteRefreshToken(userId, refreshToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the access token.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

    }
}
