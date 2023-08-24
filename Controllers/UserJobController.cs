using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserJobController : BaseController
    {
        DataContextEF _dataContext;
        public UserJobController(IConfiguration config){
            _dataContext = new DataContextEF(config);
        }

        //Get users
        [HttpGet]
        public IEnumerable<UserJobInfo> GetUsers(){
            return _dataContext.UserJobInfo.ToList();
        }

        //Get user
        [HttpGet("{id}")]
        public UserJobInfo GetUser(int id){
            UserJobInfo user = _dataContext.UserJobInfo.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("User job not found");
            if(user != null){
                return user;
            }
            throw new Exception("User job not found");
        }

        [HttpPost]
        public IActionResult CreateNewJob(UserJobInfo jobInfo){
            UserJobInfo user = _dataContext.UserJobInfo.SingleOrDefault(x => x.UserId == jobInfo.UserId);
            if(user != null) throw new Exception("User with that ID already exists");
            _dataContext.UserJobInfo.Add(jobInfo);
            if(_dataContext.SaveChanges() > 0){
                return Ok("Job created");
            }
            throw new Exception("Job not created");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateJob(int id, UserJobInfo user){
            UserJobInfo DbUser = _dataContext.UserJobInfo.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("User job not found");

            if(DbUser != null){
                DbUser.UserId = user.UserId;
                DbUser.JobTitle = user.JobTitle;
                DbUser.Department = user.Department;
                if(_dataContext.SaveChanges() > 0){
                    return Ok("Job updated");
                }
                throw new Exception("Job could not be updated");
            }
            throw new Exception("Job could not be found");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteJob(int id){
            UserJobInfo DbUser = _dataContext.UserJobInfo.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("User job not found");
            if(DbUser!=null){
                _dataContext.UserJobInfo.Remove(DbUser);
                if(_dataContext.SaveChanges()>0){
                    return Ok("Job deleted");
                }
                throw new Exception("Job could be be deleted");
            }
            throw new Exception("Job could be be found");
        }
    }
}