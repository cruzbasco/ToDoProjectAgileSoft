using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDataAccess.DataAccess;
using ToDoDataAccess.Models;

namespace ToDoDataAccess.Repositories
{

    public interface IUserTaskRespository
    {
        IEnumerable<UserTask> GetAllUserTaskByUserId(int userId);
        UserTask GetUserTaskByTaskId(int taskId);
        UserTask AddUserTask(UserTask task);
        UserTask UpdateUserTaskStateById(UserTask userTask);
        void DeleteUserTaskByTaskId(int taskId);

    }

    public class UserTaskRepository : IUserTaskRespository
    {
        private readonly ToDoContext _context;

        public UserTaskRepository(ToDoContext context)
        {
            _context = context;
        }

        public IEnumerable<UserTask> GetAllUserTaskByUserId(int userId)
        {
            return _context.UserTasks.Where(user => user.UserRefId == userId);
        }

        public UserTask GetUserTaskByTaskId(int taskId)
        {
            return _context.UserTasks.FirstOrDefault(task => task.Id == taskId);
        }
        public UserTask AddUserTask(UserTask task)
        {
            var userTask = _context.UserTasks.Add(task);
            _context.SaveChanges();
            return userTask.Entity;
        }
        public UserTask UpdateUserTaskStateById(UserTask userTask)
        {
            var foundUserTask = _context.UserTasks.FirstOrDefault(task => task.Id == userTask.Id);

            if (foundUserTask != null)
            {
                foundUserTask.TaskState = userTask.TaskState;
                foundUserTask.UpdatedDate = DateTime.Now;
            }

            _context.SaveChanges();

            return foundUserTask;
        }

        public void DeleteUserTaskByTaskId(int taskId)
        {
            var foundUserTask = _context.UserTasks.FirstOrDefault(task => task.Id == taskId);

            if (foundUserTask == null) return;

            _context.UserTasks.Remove(foundUserTask);
            _context.SaveChanges();
        }

     

       
    }
}
