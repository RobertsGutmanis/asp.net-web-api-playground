using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserSalaryController : BaseController
    {
        DataContextEF _dataContext;
        public UserSalaryController (IConfiguration config){
            _dataContext = new DataContextEF(config);
        }

        [HttpGet]
        public IEnumerable<UserSalary> GetSalaries(){
            return _dataContext.UserSalary.ToList();
        }

        [HttpGet("{id}")]
        public UserSalary GetSalaryById(int id){
            return _dataContext.UserSalary.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("Salary not found");
        }

        [HttpPost]
        public IActionResult CreateSalary(UserSalary salary){
            UserSalary user = _dataContext.UserSalary.SingleOrDefault(x => x.UserId == salary.UserId);
            if(user == null){ throw new Exception("Salary for this user already exists"); }
            _dataContext.UserSalary.Add(salary);
            if(_dataContext.SaveChanges() > 0){
                return Ok("Salary created");
            }
            throw new Exception("Salary not created!");
        }  

        [HttpPut("{id}")]
        public IActionResult UpdateSalary(int id, UserSalary salary){
            UserSalary user = _dataContext.UserSalary.SingleOrDefault(x => x.UserId == salary.UserId) ?? throw new Exception("User not found");
            if(user != null){
                user.Salary = salary.Salary;
                if(_dataContext.SaveChanges()>0){
                    return Ok("Salary updated");
                }
                throw new Exception("Salary could not be updated");
            }
            throw new Exception("Salary could not be found");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSalary(int id){
             UserSalary user = _dataContext.UserSalary.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("User not found");
             _dataContext.Remove(user);
             if(_dataContext.SaveChanges()>0){
                return Ok("Salary deleted");
             }
             throw new Exception("Salary could not be deleted");
        }
    }
}