using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleService.Models
{
    public class Person
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
