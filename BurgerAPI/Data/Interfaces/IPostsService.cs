using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Data
{
    public interface IPostsService
    {
        Task<Post> Add(Post post);
        Task<IList<Post>> GetAllUser();
        Task<IList<Post>> GetAll();
        Task<Post> Get(int postId);
        Task<Post> Delete(int postId);
        Task<Post> Update(Post post);
    }
}