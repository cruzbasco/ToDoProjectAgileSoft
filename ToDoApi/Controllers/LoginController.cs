using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDataAccess.Models;
using ToDoDataAccess.Repositories;
using ToDoTools;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRepository _repository;

        public LoginController(ILogger<LoginController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login (User user)
        {
            string encryptedPassword = Encrypt.EncryptPassword(user.Password);
            var userExist = _repository.GetUserByUsernameAndPassword(user.Username, encryptedPassword);

            if (userExist == null) 
            {
                ModelState.AddModelError("Loging Failed", "Username or password doesn't match");
                return BadRequest(ModelState);
            }
            return Ok();

        }
    }
}
