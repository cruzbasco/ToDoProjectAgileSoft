using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ToDoDataAccess.Models;
using ToDoDataAccess.Repositories;
using ToDoTools;

namespace ToDoApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _repository;

        public UserController(ILogger<UserController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            return Ok(_repository.GetAllUsers().Select(user => new UserDTO {
                Id = user.Id,
                Name = user.Name
            }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _repository.GetUserById(id);

            var userDto = new UserDTO {
                Id = user.Id,
                Name = user.Name
            };
            return Ok(userDto);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

            ValidateUserModel(user);

            if (!ModelState.IsValid) return BadRequest(ModelState);


            user.Password = Encrypt.EncryptPassword(user.Password);

            var createdUser = _repository.AddUser(user);

            return Ok(createdUser);
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

            ValidateUserModel(user);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userToUpdate = _repository.GetUserById(user.Id);

            if (userToUpdate == null) return NotFound();

            _repository.UpdateUser(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            var userToDelete = _repository.GetUserById(id);

            if (userToDelete == null) return NotFound();

            _repository.DeleteUserById(id);

            return Ok();
        }

        private void ValidateUserModel(User user)
        {
            //check fields
            if (Validation.CanNotBeEmpty(user.Username))
            {
                ModelState.AddModelError("Username", "The username shouldn't be empty");
            }

            if (Validation.CanNotBeEmpty(user.Password) && Validation.AtLeast8Characters(user.Password))
            {
                ModelState.AddModelError("Password", "The password should be at least 8 characters");
            }

            if (Validation.CanNotBeEmpty(user.Name))
            {
                ModelState.AddModelError("Name", "The name shouldn't be empty");
            }
        }
    }
}
