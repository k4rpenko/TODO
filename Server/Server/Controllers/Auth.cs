using Core.Hesh;
using Core.Models.Request;
using Core.Redis;
using Core.User;
using DALPostgresSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Auth : Controller
    {
        private readonly IRedisService _redis;
        private readonly IUser _userService;
        private readonly ILogger<Auth> _logger;
        private readonly IJWT _jwt;
        public Auth(IUser userService, ILogger<Auth> logger, IJWT jwt, IRedisService redis) 
        {
            _userService = userService;
            _logger = logger;
            _jwt = jwt;
            _redis = redis;
        }
            
        [HttpPost("registration")]
        public async Task<IActionResult> registration(UserRegisterRequest req)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var redisValid = await _redis.isAuthRedisUser(ipAddress);

                //if (!redisValid) return StatusCode(StatusCodes.Status429TooManyRequests, new { error = "Too many requests" });

                var res = await _userService.AddUserToDB(req.Name, req.Password, req.Email);

                if(res == null) return BadRequest(new { error = "Registration failed" });

                var assetsToken = _jwt.GenerateToken(res.Id.ToString(),  res.Email,  TokenType.Access);
                var refreshToken = _jwt.GenerateToken(res.Id.ToString(), res.Email,  TokenType.Refresh);

                await _userService.AddRefreshToken(res.Id.ToString(), refreshToken);

                Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Path = "/"
                });

                return Ok(new { Token = assetsToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user {Email}", req.Email);

                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginRequest req)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                var redisValid = await _redis.isAuthRedisUser(ipAddress);

                //if (!redisValid) return StatusCode(StatusCodes.Status429TooManyRequests, new { error = "Too many requests" });

                var userId = await _userService.CheckUserInDB(req.Email);

                if (userId == null) return BadRequest(new { error = "User not found" });

                var user = await _userService.GetUserById(userId);

                var pass = await _userService.CheckPassword(req.Password, user.PasswordHash, user.Salt);

                if(!pass) return BadRequest(new { error = "Wrong password" });


                var assetsToken = _jwt.GenerateToken(user.Id.ToString(),  user.Email,  TokenType.Access);
                var refreshToken = _jwt.GenerateToken(user.Id.ToString(), user.Email, TokenType.Refresh);

                await _userService.AddRefreshToken(user.Id.ToString(), refreshToken);

                Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Path = "/"
                });

                return Ok(new { Token = assetsToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user {Email}", req.Email);

                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    
    }
}
