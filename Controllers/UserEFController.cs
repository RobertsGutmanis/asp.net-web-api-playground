using API.Data;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersEFController : BaseController
    {
        DataContextEF _dataContext;
        IMapper _mapper;
        public UsersEFController(IConfiguration config){
            _dataContext = new DataContextEF(config);
            _mapper = new Mapper(new MapperConfiguration(config => {
                config.CreateMap<UserDTO, User>();
            }));
        }
        
        // Get Users
        [HttpGet]
        public IEnumerable<User> GetUsers(){
            return _dataContext.Users.ToList();
        }

        // Get User
        [HttpGet("{id}")]
        public User GetUserById(int id){
            return _dataContext.Users.Single(u => u.UserId == id);
        }

        //Create user
        [HttpPost]
        public IActionResult CreateUser(User user){
            User DbUser = _mapper.Map<User>(user);
            _dataContext.Users.Add(DbUser);
            if(_dataContext.SaveChanges() >0){
                return Ok("User created");
            }
            throw new Exception("Failed to create user");

        }

        //Update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(UserDTO user, int id){
            User DbUser = _dataContext.Users.Single(u => u.UserId == id);

            if(DbUser != null){
                DbUser.FirstName = user.FirstName;
                DbUser.LastName = user.LastName;
                DbUser.Email = user.Email;
                DbUser.Gender = user.Gender;
                DbUser.Active = user.Active;
                if(_dataContext.SaveChanges() > 0){
                     return Ok("User updated");

                }
                throw new Exception("Failed to update user");
            }
            throw new Exception("Failed to find user");
        }

        //Delete user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id){
            User DbUser = _dataContext.Users.Single(u => u.UserId == id);
            _dataContext.Users.Remove(DbUser);
            if(_dataContext.SaveChanges() > 0){
                return Ok("User deleted");
            }
            throw new Exception("Failed to delete user");
        }
    }
}