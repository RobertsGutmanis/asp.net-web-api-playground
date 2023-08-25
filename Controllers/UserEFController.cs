using API.Data;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersEFController : BaseController
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        public UsersEFController(IConfiguration config, IUserRepository userRepository){
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(config => {
                config.CreateMap<UserDTO, User>();
            }));
        }
        
        // Get Users
        [HttpGet]
        public IEnumerable<User> GetUsers(){
            return _userRepository.GetUsers();
        }

        // Get User
        [HttpGet("{id}")]
        public User GetUserById(int id){
            return _userRepository.GetUserById(id);
        }

        //Create user
        [HttpPost]
        public IActionResult CreateUser(User user){
            User DbUser = _mapper.Map<User>(user);
            _userRepository.AddEntity(DbUser);
            if(_userRepository.SaveChanges()){
                return Ok("User created");
            }
            throw new Exception("Failed to create user");

        }

        //Update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(UserDTO user, int id){
            User DbUser = _userRepository.GetUserById(id);

            if(DbUser != null){
                DbUser.FirstName = user.FirstName;
                DbUser.LastName = user.LastName;
                DbUser.Email = user.Email;
                DbUser.Gender = user.Gender;
                DbUser.Active = user.Active;
                if(_userRepository.SaveChanges()){
                     return Ok("User updated");
                }
                throw new Exception("Failed to update user");
            }
            throw new Exception("Failed to find user");
        }

        //Delete user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id){
            User DbUser = _userRepository.GetUserById(id);
            _userRepository.RemoveEntity(DbUser);
            if(_userRepository.SaveChanges()){
                return Ok("User deleted");
            }
            throw new Exception("Failed to delete user");
        }
    }
}