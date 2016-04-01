using NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MemoryRepsitoryTests
    {
        IRepository repository = new MemoryRepository();

        public MemoryRepsitoryTests()
        {
            dynamic John = new ExpandoObject();
            John.Name = "John Smith";
            dynamic Jane = new ExpandoObject();
            Jane.Name = "Jane Doe";
            repository.Create("Person", John, Jane).Wait();
        }

        [Fact]
        public void RetrievePeopleTest()
        {
            var people = repository.GetAll("Person").Result;
            Assert.True(people.Count() == 2);
            Assert.True(people.Any(x => x.Name == "John Smith"));
            Assert.True(people.Any(x => x.Name == "Jane Doe"));
        }

        [Fact]
        public void RetrievePersonByIdTest()
        {
            var ids = repository.GetAll("Person").Result.Select(x => (Guid)(x.Id)).ToArray();
            var person = repository.GetById("Person", ids).Result;
            Assert.True(person.Count() == 2);
        }
    }
}
