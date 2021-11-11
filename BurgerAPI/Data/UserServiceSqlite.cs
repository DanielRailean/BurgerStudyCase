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
    public class UserServiceSqlite : IUserService
    {
        private UserCredentialsDbContext dbContext;
        private IAuthService _authService;

        public UserServiceSqlite(UserCredentialsDbContext dbContext, IAuthService authService)
        {
            this.dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IList<User>> GetAllUsers()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User> Register(User user)
        {
            
            var userSalt = CreateSalt();
            user.Salt = userSalt;
            user.Password = Convert.ToBase64String(HashPasswordV1(user.Password,userSalt));
            user.HashVersion = Environment.GetEnvironmentVariable("PASSWORD_HASH_VERSION");
            user.ApiVersion = Environment.GetEnvironmentVariable("API_VERSION");
            user.RegistrationDate = DateTime.Now;
            user.EmailConfirmed = false;
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            user.Salt = null;
            user.Password = null;
            user.HashVersion = null;
            return user;
        }
        

        public async Task<User> Delete()
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            User toRemove = await dbContext.Users.FirstAsync(f => f.Id == _authService.AuthModel.UserId);
            dbContext.Remove(toRemove);
            await dbContext.SaveChangesAsync();
            return toRemove;
        }

        public async Task<User> Validate(string email, string password)
        {
            User found = await dbContext.Users.FirstAsync(f => f.Email.Equals(email));
            if (VerifyHash(password,found.Salt,found.Password))
                return found;
            
            throw new Exception("Password or email incorrect");
        }
        
        public async Task<User> Update(User user)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            if(user.Id!=_authService.AuthModel.UserId)
            {
                throw new Exception("Unauthorized");
            }
            User toUpdate = await dbContext.Users.FirstAsync(f => f.Id == user.Id);
            toUpdate.Email = user.Email;
            toUpdate.Name = user.Name;
            dbContext.Update(toUpdate);
            await dbContext.SaveChangesAsync();
            return toUpdate;
        }
        
        private string CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        private byte[] HashPasswordV1(string password, string salt)
        {
            var decodedSalt = Convert.FromBase64String(salt);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = decodedSalt;
            argon2.DegreeOfParallelism = 1; // four cores
            argon2.Iterations = 2;
            argon2.MemorySize = 256*256; // 1 GB

            return argon2.GetBytes(16);
        }
        
        private bool VerifyHash(string password, string salt, string hash)
        {
            var decodedHash = Convert.FromBase64String(hash);
            var newHash = HashPasswordV1(password, salt);
            return decodedHash.SequenceEqual(newHash);
        }
        
    }
}