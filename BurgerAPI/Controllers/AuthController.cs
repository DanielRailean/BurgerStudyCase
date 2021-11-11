using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;
        private ITokenService tokenService;
       
        public AuthController(IAuthService AuthService, ITokenService tokenService)
        {
            this.authService = AuthService;
            this.tokenService = tokenService;
        }
        
        /*[HttpGet]
        [Route("access")]
        public async Task<ActionResult<string>> GetAuths()
        {
            try
            {
                string token = Request.Cookies["rt"];
                var payload = await authService.GetPayloadRefresh(token);
                if (await tokenService.ContainsToken(payload))
                {
                    // access token valable for 15 min ( 15 * 60 sec)
                    var newPayload = new AuthModel(payload.UserId, 15*60);
                    var dict = new Dictionary<string, string>();
                    var accessToken = await authService.GenerateAccessToken(newPayload);
                    dict.Add("token", accessToken);
                    return Ok(dict);
                }
                return StatusCode(401,new ApiError("Error with refresh token"));

            }
            catch (Exception e)
            {
                return StatusCode(401, new ApiError(e.Message));
            }
            
        }*/
        /*[HttpPost]
        [Route("payload")]
        public async Task<ActionResult<string>> AddAuth()
        {
            try
            {
                string token = Request.Headers["Authorization"];
                token = token.Split(" ")[1];
                return Ok(await AuthService.GetPayload(token));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
        }*/
    }
}