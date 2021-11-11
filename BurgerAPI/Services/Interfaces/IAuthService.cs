using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public interface IAuthService
    {
        public AuthModel AuthModel { get; set; }
        public bool IsTokenValid { get; set; }
        Task<string> GenerateAccessToken(AuthModel model);
        Task<string> GenerateRefreshToken(AuthModel model);
        Task<AuthModel> GetPayloadAccess(string token);
        Task<AuthModel> GetPayloadRefresh(string token);
    }
}