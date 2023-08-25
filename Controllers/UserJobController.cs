using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserJobController : BaseController
    {
        IUserRepository _userRepository;
        public UserJobController(IUserRepository userRepository){
            _userRepository = userRepository;
        }

        //Get users
        [HttpGet]
        public IEnumerable<UserJobInfo> GetUserJobInfo(){
            return _userRepository.GetUserJobInfo();
        }

        //Get user
        [HttpGet("{id}")]
        public UserJobInfo GetUser(int id){
            return _userRepository.GetOneJob(id);
        }

        [HttpPost]
        public IActionResult CreateNewJob(UserJobInfo jobInfo){
            _userRepository.AddEntity(jobInfo);
            if(_userRepository.SaveChanges()){
                return Ok("Job created");
            }
            throw new Exception("Job not created");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateJob(int id, UserJobInfo user){
            UserJobInfo DbUser = _userRepository.GetOneJob(id);

            if(DbUser != null){
                DbUser.UserId = user.UserId;
                DbUser.JobTitle = user.JobTitle;
                DbUser.Department = user.Department;
                if(_userRepository.SaveChanges()){
                    return Ok("Job updated");
                }
                throw new Exception("Job could not be updated");
            }
            throw new Exception("Job could not be found");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteJob(int id){
            UserJobInfo DbUser = _userRepository.GetOneJob(id);
            if(DbUser!=null){
                _userRepository.RemoveEntity(DbUser);
                if(_userRepository.SaveChanges()){
                    return Ok("Job deleted");
                }
                throw new Exception("Job could be be deleted");
            }
            throw new Exception("Job could be be found");
        }
    }
}