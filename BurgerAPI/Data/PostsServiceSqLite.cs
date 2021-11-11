using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MoneyTrackDatabaseAPI.DataAccess;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Data
{
    public class PostServiceSqlite : IPostsService
    {
        private UserContentDbContext dbContext;
        private IAuthService _authService;
        private SecurityService _securityService;

        public PostServiceSqlite(UserContentDbContext dbContext, IAuthService authService, SecurityService securityService)
        {
            this.dbContext = dbContext;
            _authService = authService;
            _securityService = securityService;
        }

        public async Task<IList<Post>> GetAllUser()
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            return await dbContext.Posts.Where(u=>u.AuthorId==_authService.AuthModel.UserId).ToListAsync();
        }

        public async Task<IList<Post>> GetAll()
        {
            return await dbContext.Posts.ToListAsync();
        }

        public async Task<Post> Get(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> Add(Post post)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }

            post.AuthorId = _authService.AuthModel.UserId;
            await dbContext.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post> Delete(int id)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            Post toRemove = await dbContext.Posts.FirstAsync(f => f.Id == id);
            if (toRemove.AuthorId != _authService.AuthModel.UserId)
            {
                throw new Exception("Access denied!");
            }
            dbContext.Remove(toRemove);
            await dbContext.SaveChangesAsync();
            return toRemove;
        }
        
        
        public async Task<Post> Update(Post post)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            Post toUpdate = await dbContext.Posts.FirstAsync(f => f.Id == post.Id);
            if(post.AuthorId!=_authService.AuthModel.UserId)
            {
                throw new Exception("Unauthorized");
            }
            toUpdate.Title = post.Title;
            toUpdate.RestaurantId = post.RestaurantId;
            toUpdate.Content = post.Content;
            toUpdate.ImageUrl = post.ImageUrl;
            toUpdate.TasteScore = post.TasteScore;
            toUpdate.TextureScore = post.TextureScore;
            toUpdate.VisualScore = post.VisualScore;
            
            dbContext.Update(toUpdate);
            await dbContext.SaveChangesAsync();
            return toUpdate;
        }
        
        
    }
}