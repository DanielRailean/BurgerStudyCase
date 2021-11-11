using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private IAuthService authService;
        private ITokenService tokenService;

        private int refreshTokenTTL;
        private int accessTokenTTL;

        public UserController(IUserService userService, IAuthService authService,ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.authService = authService;
            refreshTokenTTL = int.Parse(Environment.GetEnvironmentVariable("REFRESH_TOKEN_TTL"));
            accessTokenTTL = int.Parse(Environment.GetEnvironmentVariable("ACCESS_TOKEN_TTL"));

        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            try
            {
                await userService.Register(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }

        [HttpDelete]
        [Route("deleteProfile")]
        public async Task<ActionResult<User>> RemoveUser()
        {
            try
            {
                var returnedUser = await userService.Delete();
                return Ok(returnedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            try
            {
                var returnedUser = await userService.Validate(user.Email,user.Password);
                var model = new AuthModel(returnedUser.Id, refreshTokenTTL);
                await tokenService.AddToken(model);
                var token = await authService.GenerateRefreshToken(model);
                var newPayload = new AuthModel(returnedUser.Id, accessTokenTTL);
                var dict = new Dictionary<string, string>();
                var accessToken = await authService.GenerateAccessToken(newPayload);
                dict.Add("token", accessToken);
                CookieOptions cookieOptions = new CookieOptions();
                //TODO Uncomment before production
                cookieOptions.Secure = true;
                cookieOptions.HttpOnly = true;
                cookieOptions.SameSite = SameSiteMode.None;
                HttpContext.Response.Cookies.Append("rt",token,cookieOptions);
                return Ok(dict);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
        [HttpGet]
        [Route("refresh")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                string token = Request.Cookies["rt"];
                if (token is "")
                {
                    return StatusCode(401,new ApiError("You are not logged in!"));
                }
                var payload = await authService.GetPayloadRefresh(token);
                if (!await tokenService.ContainsToken(payload))
                    return StatusCode(401, new ApiError("Refresh token Invalidated!"));
                
                var newPayload = new AuthModel(payload.UserId, accessTokenTTL);
                var dict = new Dictionary<string, string>();
                var accessToken = await authService.GenerateAccessToken(newPayload);
                dict.Add("token", accessToken);
                return Ok(dict);

            }
            catch (Exception e)
            {
                return StatusCode(401, new ApiError(e.Message));
            }
            
        }
        [HttpGet]
        [Route("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            try
            {
                string token = Request.Cookies["rt"];
                if (token is "")
                {
                    return StatusCode(401,new ApiError("You are not logged in!"));
                }
                var payload = await authService.GetPayloadRefresh(token);
                
                await tokenService.Logout(payload);

                CookieOptions cookieOptions = new CookieOptions
                {
                    Secure = true, HttpOnly = true, SameSite = SameSiteMode.None
                };
                HttpContext.Response.Cookies.Append("rt","",cookieOptions);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(401, new ApiError(e.Message));
            }
            
        }
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromBody]User user)
        {
            try
            {
                await userService.Update(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
    }
}