using API.Models;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _dataContext;
        public UserRepository(IConfiguration config){
            _dataContext = new DataContextEF(config);
        }
        public bool SaveChanges(){
            return _dataContext.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd){
            if(entityToAdd != null){
                _dataContext.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove){
            if(entityToRemove != null){
                _dataContext.Remove(entityToRemove);
            }
        }
        
        public IEnumerable<User> GetUsers(){
            return _dataContext.Users.ToList();
        }

         public User GetUserById(int id){
            User user =  _dataContext.Users.Single(u => u.UserId == id);
            if(user != null){
                return user;
            }
            throw new Exception("User job not found");
        }

        public IEnumerable<UserJobInfo> GetUserJobInfo(){
            return _dataContext.UserJobInfo.ToList();
        }
        public UserJobInfo GetOneJob(int id){
            return _dataContext.UserJobInfo.SingleOrDefault(x => x.UserId == id) ?? throw new Exception("User job not found");
        }
    }
}