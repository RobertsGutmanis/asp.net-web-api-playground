using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly AuthHelper auth;
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config){
            _dapper = new DataContextDapper(config);
            _config = config;
            auth = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegistrationDTO model){
            if(model.Password == model.PasswordConfirm){
                string sqlCheckEmail = $"SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '{model.Email}'";
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckEmail);
                if(existingUsers.Count() == 0){

                    byte[] PasswordSalt = new byte[128/8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create()){
                        rng.GetNonZeroBytes(PasswordSalt);
                    }
                    byte[] PasswordHash = auth.GetPasswordHash(model.Password, PasswordSalt);

                    string sqlAddAuth = $@"INSERT INTO TutorialAppSchema.Auth 
                    (Email, PasswordHash, PasswordSalt) 
                    VALUES ('{model.Email}', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    SqlParameter passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);

                    passwordSaltParam.Value = PasswordSalt;
                    passwordHashParam.Value = PasswordHash;

                    sqlParameters.Add(passwordSaltParam);
                    sqlParameters.Add(passwordHashParam);

                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters)){
                        string sql = $@"INSERT INTO TutorialAppSchema.Users (FirstName, LastName, Email, Gender, Active)
                        VALUES ('{model.FirstName}', '{model.LastName}', '{model.Email}', '{model.Gender}', 1)";
                        if(_dapper.ExecuteSql(sql)){
                            return Ok("User created");
                        }
                        throw new Exception("User could not be stored");
                    }
                    throw new Exception("User could not be created");
                }
                throw new Exception("User with that email exists");
            }
            throw new Exception("Passwords don't match");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginDTO model){
            string sqlData = $@"SELECT PasswordHash, PasswordSalt FROM TutorialAppSchema.Auth WHERE Email = '{model.Email}'";

            ConfirmationDTO user = _dapper.LoadDataSingle<ConfirmationDTO>(sqlData);

            byte[] PasswordHash = auth.GetPasswordHash(model.Password, user.PasswordSalt);

            for(int index = 0; index < PasswordHash.Length; index++){
                if(PasswordHash[index] != user.PasswordHash[index]){
                    return StatusCode(401, "Incorrect password");
                }
            }
            string userIdSql = $"SELECT userId FROM TutorialAppSchema.Users WHERE email = '{model.Email}'";
            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", auth.CreateToken(userId)}
            });
        }

        [HttpGet("refresh")]
        public string RefreshToken(){
            string sqlGetUserId = $"SELECT userId FROM TutorialAppSchema.Users WHERE userId = '{User.FindFirst("userId").Value}'";

            int userId = _dapper.LoadDataSingle<int>(sqlGetUserId);

            return auth.CreateToken(userId);
        }
    }
}