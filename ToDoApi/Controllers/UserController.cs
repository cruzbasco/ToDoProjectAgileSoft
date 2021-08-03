using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public IActionResult GetAllUsers()
        {
            return Ok(_repository.GetAllUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(_repository.GetUserById(id));
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

             //check fields
            if (Validation.CanNotBeEmpty(user.Username))
            {
                ModelState.AddModelError("Username", "The username shouldn't be empty");
            }

            if (Validation.CanNotBeEmpty(user.Password) || Validation.AtLeast8Characters(user.Password))
            {
                ModelState.AddModelError("Password", "The password should be at least 8 characters");
            }

            if (Validation.CanNotBeEmpty(user.Name))
            {
                ModelState.AddModelError("Name", "The name shouldn't be empty");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);


            user.Password = Encrypt.EncryptPassword(user.Password);

            var createdUser = _repository.AddUser(user);

            return Ok(createdUser);
        }

        

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

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

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userToUpdate = _repository.GetUserById(user.Id);

            if (userToUpdate == null) return NotFound();

            _repository.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            var userToDelete = _repository.GetUserById(id);

            if (userToDelete == null) return NotFound();

            _repository.DeleteUserById(id);

            return NoContent();
        }
    }
}
