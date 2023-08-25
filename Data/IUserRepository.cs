using API.Models;

namespace API.Data
{
    public interface IUserRepository
    {
         public bool SaveChanges();
         public void AddEntity<T>(T entityToAdd);
         public void RemoveEntity<T>(T entityToRemove);
         public IEnumerable<User> GetUsers();
         public User GetUserById(int id);
         public IEnumerable<UserJobInfo> GetUserJobInfo();
         public UserJobInfo GetOneJob(int id);
         
    }
}