using Azure;
using Azure.Core;
using IdentityServer.Models.DTOs;
using IdentityServer.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginRequest request)
        //{
        //    var response = _authService.Login(request);
        //    return StatusCode((int)response.StatusCode, response);
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //if (ModelState.IsValid)
            //{

            //}
                var response = _authService.Login(request);

            if (!response.IsSuccess)
                return StatusCode((int)response.StatusCode, response);

            var loginResponse = response.Result as LoginResponse;
            if (loginResponse != null)
            {
                // Set Access Token vào HttpOnly Cookie
                Response.Cookies.Append("accessToken", loginResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                Response.Cookies.Append("refreshToken", loginResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(3)
                });
                //response.Result = "Logged in successfully";
            }

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var response = _authService.Register(request);
            return StatusCode((int)response.StatusCode, response);
        }

        //[HttpPost("refresh")]
        //public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        //{
        //    var response = _authService.RefreshToken(request.RefreshToken);
        //    return StatusCode((int)response.StatusCode, response);
        //}
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _authService.RefreshToken(refreshToken);
            var accessToken = ((LoginResponse)response.Result).AccessToken;
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            //return Ok(response);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _authService.Logout(refreshToken);
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), // Đặt thời gian hết hạn trong quá khứ để đảm bảo cookies bị xóa
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
