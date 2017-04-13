using PeopleUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleUI.Services
{
    public interface IPeopleService
    {
        Task<List<Person>> GetPeople();
    }
}
