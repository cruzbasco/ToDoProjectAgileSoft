using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDataAccess.Models;

namespace ToDoDataAccess.DataAccess
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }

    }
}
