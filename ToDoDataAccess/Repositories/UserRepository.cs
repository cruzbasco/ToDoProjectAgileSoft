using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDataAccess.DataAccess;
using ToDoDataAccess.Models;

namespace ToDoDataAccess.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);
        User GetUserByUsernameAndPassword(string username, string password);
        User AddUser(User user);
        User UpdateUser(User updatedUser);
        void DeleteUserById(int userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ToDoContext _context;
        public UserRepository(ToDoContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(user => user.Id == userId);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(user => user.Username == username && user.Password == password);
        }


        public User AddUser(User user)
        {
            var addUser = _context.Users.Add(user);
            _context.SaveChanges();
            return addUser.Entity;
        }
        public User UpdateUser(User updatedUser)
        {

            var foundUser = _context.Users.FirstOrDefault(user => user.Id == updatedUser.Id);
            if (foundUser != null)
            {
                foundUser.Username = updatedUser.Username;
                foundUser.Password = updatedUser.Password;
                foundUser.Name = updatedUser.Name;
            }

            _context.SaveChanges();

            return foundUser;
        }

        public void DeleteUserById(int userId)
        {
            var foundUser = _context.Users.FirstOrDefault(user => user.Id == userId);

            if (foundUser == null) return;

            _context.Users.Remove(foundUser);
            _context.SaveChanges();
        }

     

        
    }
}
