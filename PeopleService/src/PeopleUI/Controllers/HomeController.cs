using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleUI.Models;
using PeopleUI.Services;

namespace PeopleUI.Controllers
{
    public class HomeController : Controller
    {
        private IPeopleService _peopleService;

        public HomeController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public async Task<IActionResult> Index()
        {
            var people = await _peopleService.GetPeople();
            return View(people);
        }
    
    }
}
