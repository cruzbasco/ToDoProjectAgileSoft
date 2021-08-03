using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDataAccess.Models;
using ToDoDataAccess.Repositories;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTaskController : Controller
    {
        private readonly ILogger<UserTaskController> _logger;
        private readonly IUserTaskRespository _repository;

        public UserTaskController(ILogger<UserTaskController> logger, IUserTaskRespository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{userId}")]
        public IActionResult GetAllUserTasksByUserId(int userId)
        {
            return Ok(_repository.GetAllUserTaskByUserId(userId));
        }

        [HttpPost]
        public IActionResult AddUserTask([FromBody] UserTask task)
        {
            if (task == null) return BadRequest();

            var createdUser = _repository.AddUserTask(task);

            return Ok(createdUser);
        }

        [HttpPut]
        public IActionResult UpdateUserTask([FromBody] UserTask task)
        {
            if (task == null) return BadRequest();

            var userToUpdate = _repository.GetUserTaskByTaskId(task.Id);

            if (userToUpdate == null) return NotFound();

            _repository.UpdateUserTaskById(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserTask(int id)
        {
            if (id == 0) return BadRequest();

            var userToDelete = _repository.GetUserTaskByTaskId(id);

            if (userToDelete == null) return NotFound();

            _repository.DeleteUserTaskByTaskId(id);

            return NoContent();
        }

    }
}
