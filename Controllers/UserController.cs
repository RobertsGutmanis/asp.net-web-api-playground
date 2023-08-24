using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        DataContextDapper _dataContext;
        public UserController(IConfiguration config){
            _dataContext = new DataContextDapper(config);
        }
////////////////////////////////////////////////////////////////API

        // Gets all users
        [HttpGet]
        public IEnumerable<User> GetUsers(){
            return _dataContext.LoadData<User>("SELECT * FROM TutorialAppSchema.Users");
        }

        // Gets one user
        [HttpGet("{id}")]
        public User GetUserById(int id){
            return _dataContext.LoadDataSingle<User>($"SELECT * FROM TutorialAppSchema.Users WHERE UserId = {id}");
        }

        // Edits user
        [HttpPut("{id}")]
        public IActionResult EditUser(int id, UserDTO user){
            if(_dataContext.ExecuteSql($@"UPDATE TutorialAppSchema.Users 
            SET FirstName = '{user.FirstName}', LastName = '{user.LastName}',
             Email = '{user.Email}', Gender = '{user.Gender}', Active = '{user.Active}'
            WHERE UserId = {id}")){
                return Ok("User updated!");
            }
            throw new Exception("Failed to update user!");
        }

        // Creates user
        [HttpPost]
        public IActionResult AddUser(UserDTO user){
            if(_dataContext.ExecuteSql($@"INSERT INTO TutorialAppSchema.Users 
            (FirstName, LastName, Email, Gender, Active) 
            VALUES
            ('{user.FirstName}','{user.LastName}','{user.Email}','{user.Gender}','{user.Active}')")){
                return Ok("User created!");
            }
            throw new Exception("Failed to update user!");
        }

        //Deletes user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id){
            if(_dataContext.ExecuteSql($@"DELETE FROM TutorialAppSchema.Users WHERE UserId = {id}")){
                return Ok("User deleted!");
            }
            throw new Exception("Failed to update user!");
        }
    }
}