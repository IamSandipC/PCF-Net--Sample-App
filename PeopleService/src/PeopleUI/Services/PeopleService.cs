using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeopleUI.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Pivotal.Discovery.Client;

namespace PeopleUI.Services
{
    public class PeopleService : BaseDiscoveryService, IPeopleService
    {
        

        const string peopleserviceURI= "http://peopleservice/api/people";

        public PeopleService(IDiscoveryClient client) : base(client)
        {
        }

        public async Task<List<Person>> GetPeople()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, peopleserviceURI);

            var people = await Invoke<List<Person>>(request);

            return people;
        }
    }
}
