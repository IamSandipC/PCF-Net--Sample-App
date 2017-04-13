using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleService.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PeopleService.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        public PeopleController(PeopleContext dbContext)
        {
            DbContext = dbContext;
        }

        public PeopleContext DbContext { get; }



        // GET: /api/People
        [HttpGet("")]
        public async Task<List<Person>> GetGenres()
        {
            var people = await DbContext.People
                .ToListAsync();

            return people;
        }

    }
}
