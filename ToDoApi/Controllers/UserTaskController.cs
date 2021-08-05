using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
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


        [HttpGet]
        [Authorize]
        public IActionResult GetAllUserTasks()
        {
            int userId = GetUserId();

            if (userId == 0) return BadRequest();

            return Ok(_repository.GetAllUserTaskByUserId(userId).Select( dto => new UserTaskDTO { 
                Id = dto.Id,
                Name = dto.Name,
                TaskState = dto.TaskState,
                Description = dto.Description,
            }));
        }

        

        [HttpPost]
        [Authorize]
        public IActionResult AddUserTask([FromBody] UserTask task)
        {
            if (task == null) return BadRequest();

            int userId = GetUserId();

            task.UserRefId = userId;

            var createdUser = _repository.AddUserTask(task);

            return Ok(createdUser);
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUserTask([FromBody] UserTask task)
        {
            if (task == null) return BadRequest();


            var taskToUpdate = _repository.GetUserTaskByTaskId(task.Id);

            if (taskToUpdate == null) return NotFound();

            if (CheckUserId(taskToUpdate.UserRefId)) return Unauthorized();

            _repository.UpdateUserTaskStateById(task);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUserTask(int id)
        {
            if (id == 0) return BadRequest();

            var taskToUpdate = _repository.GetUserTaskByTaskId(id);

            if (taskToUpdate == null) return NotFound();

            if (CheckUserId(taskToUpdate.UserRefId)) return Unauthorized();

            _repository.DeleteUserTaskByTaskId(id);

            return Ok();
        }

        private bool CheckUserId(int userRefId)
        {
            int userId = GetUserId();

            return userRefId != userId;
        }

        private int GetUserId()
        {
            string id = User.FindFirst(ClaimTypes.Sid)?.Value ?? "0";

            return int.Parse(id);
        }
    }
}
