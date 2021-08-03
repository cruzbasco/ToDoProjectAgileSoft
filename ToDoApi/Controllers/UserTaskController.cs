using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Controllers
{
    public class UserTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
