using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleService.Models
{
    public class Sample
    {

        public async static Task InitializeMusicStoreDatabase(IServiceProvider serviceProvider)
        {
            if (ShouldDropCreateDatabase())
            {

                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var db = serviceScope.ServiceProvider.GetService<PeopleContext>();

                    if (db.Database.EnsureCreated())
                    {
                        await InsertTestData(serviceProvider);

                    }
                }
            }

        }

        private async static Task InsertTestData(IServiceProvider serviceProvider)
        {
            await AddOrUpdateAsync(serviceProvider, a => a.Name, People.Select(person => person.Value));
        }


        private static Dictionary<string, Person> people;
        public static Dictionary<string, Person> People
        {
            get
            {
                if (people == null)
                {
                    var peopleList = new Person[]
                    {
                        new Person { Name = "Taylor  Chapman", Location="NY" },
                        new Person { Name = "Marlon  Jenkins", Location="FL"  },
                        new Person { Name = "Carol   Garcia", Location="CA"  },
                        new Person { Name = "Anthony Diaz", Location="NY" },
                        new Person { Name = "Vera    Clayton", Location="FL"  },
                        new Person { Name = "Meghan  Castro", Location="CA"  }
                    };

                    people = new Dictionary<string, Person>();
                    foreach (Person person in peopleList)
                    {
                        people.Add(person.Name, person);
                    }
                }

                return people;
            }
        }

        private async static Task AddOrUpdateAsync<TEntity>(
          IServiceProvider serviceProvider,
          Func<TEntity, object> propertyToMatch, IEnumerable<TEntity> entities)
          where TEntity : class
        {
            // Query in a separate context so that we can attach existing entities as modified
            List<TEntity> existingData;
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<PeopleContext>();
                existingData = db.Set<TEntity>().ToList();
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<PeopleContext>();
                foreach (var item in entities)
                {
                    //var exists = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)));
                    //if (!exists)
                    //    db.Entry(item).State = EntityState.Added;

                    db.Entry(item).State = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)))
                       ? EntityState.Modified
                       : EntityState.Added;
                }

                await db.SaveChangesAsync();
            }
        }

        private static bool ShouldDropCreateDatabase()
        {
            string index = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX");
            if (string.IsNullOrEmpty(index))
            {
                return true;
            }
            int indx = -1;
            if (int.TryParse(index, out indx))
            {
                if (indx > 0) return false;
            }
            return true;
        }
    }
}
