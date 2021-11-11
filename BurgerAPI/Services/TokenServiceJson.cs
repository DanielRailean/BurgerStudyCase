using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Services
{
    public class TokenServiceJson : ITokenService
        {
            private string TokensFile = "Tokens.json";
            private IList<AuthModel> AllTokens;

            public TokenServiceJson()
            {
                if (File.Exists(TokensFile))
                {
                    string TokensInJSON = File.ReadAllText(TokensFile);
                    AllTokens = JsonSerializer.Deserialize<IList<AuthModel>>(TokensInJSON);
                }
                else
                {
                    Seed();
                    Save();
                }
            }
            private void Seed()
            {
                IList<AuthModel> Tokens = new List<AuthModel>();
                AllTokens = Tokens.ToList();
            }

            private void Save()
            {
                string TokensInJson = JsonSerializer.Serialize(AllTokens);
                File.WriteAllText(TokensFile, TokensInJson);
            }

            public async Task AddToken(AuthModel token)
            {
                AllTokens.Add(token);
                Save();
            }

            public async Task RemoveAllForUser(int userId)
            {
                var check = AllTokens.Where((s => s.UserId==userId)).ToList();
                Console.WriteLine(JsonSerializer.Serialize(check));
                foreach (var item in check)
                {
                    AllTokens.Remove(item);
                    Save();
                }
                
            }

            public async Task Logout(AuthModel token)
            {
                var check = AllTokens.FirstOrDefault(s => s.UserId==token.UserId && s.exp==token.exp);
                if (check != null)
                {
                    AllTokens.Remove(check);
                    Save();
                }
                else
                {
                    throw new Exception("Token not found!");
                }
            }

            public async Task<bool> ContainsToken(AuthModel token)
            {
                var check = AllTokens.FirstOrDefault((s => s.UserId==token.UserId&&s.exp==token.exp));
                if(check!=null)
                {
                    return true;
                }
                return false;
            }
        }
}