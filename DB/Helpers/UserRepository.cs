using DB.Models;

namespace DB.Helpers
{
    public interface IUserRepository
    {
        User GetUserById(string id);
        User GetUserByEmail(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUserById(string id)
        {
            return _dbContext.Users.Find(id);
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
        }
    }

}
