using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public interface ITokenService
    {
        Task AddToken(AuthModel token);
        Task RemoveAllForUser(int userId);
        Task<bool> ContainsToken(AuthModel token);
        Task Logout(AuthModel token);
    }
}