using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class AuthHelper
    {
        IConfiguration _config;

        public AuthHelper(IConfiguration config){
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] passwordSalt){
            string passwordSaltString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256/8
            );

            return passwordHash;
        }

        public string CreateToken(int userId){
            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString()),
            };
            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
            SymmetricSecurityKey TokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                tokenKeyString != null ? tokenKeyString : ""
            ));

            SigningCredentials credentials = new SigningCredentials(TokenKey, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(7)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}