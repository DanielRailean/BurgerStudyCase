using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }
        
        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IList<Post>>> GetPostsOfUser()
        {
            try
            {
                IList<Post> posts = await _postsService.GetAllUser();
                return Ok(posts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
            
        }
        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                Content = new StringContent("Invalid post"),
                ReasonPhrase = "Invalid request"
            });
            try
            {
                var returned = await _postsService.Add(post);
                return Ok(returned);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
            
        }

        [HttpDelete]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> RemovePost([FromRoute] int postId)
        {
            try
            {
                var returnedPost = await _postsService.Delete(postId);
                return Ok(returnedPost);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
        }
        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int postId)
        {
            try
            {
                var returnedPost = await _postsService.Get(postId);
                return Ok(returnedPost);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
        }
        
        [HttpPut]
        public async Task<ActionResult<Post>> UpdatePost([FromBody]Post post)
        {
            try
            {
                var returned = await _postsService.Update(post);
                return Ok(returned);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(403, new ApiError(e.Message));
            }
        }
    }   
}