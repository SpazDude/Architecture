using NoSql.Repositories;
using System;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MemoryRepsitoryTests
    {
        IRepository repository = new MemoryRepository(new TestIdFactory());
        Guid[] ids = new Guid[0];

        public MemoryRepsitoryTests()
        {
            dynamic John = new ExpandoObject();
            John.Name = "John Smith";
            dynamic Jane = new ExpandoObject();
            Jane.Name = "Jane Doe";
            dynamic Guy = new ExpandoObject();
            Guy.Name = "I will be deleted";

            ids = repository.Create("Person", John, Jane, Guy).Result;

            repository.Delete("Person", ids[2]);

            dynamic NewJane = new ExpandoObject();
            NewJane.Id = ids[1];
            NewJane.Name = "Jane Buck";
            repository.Update("Person", NewJane);
        }

        [Fact]
        public void RetrievePeopleTest()
        {
            var people = repository.GetAll("Person").Result;
            Assert.True(people.Count() == 2);            
        }

        [Fact]
        public void RetrievePersonByIdTest()
        {
            var id = new Guid("00000000000000000000000000000001");
            var person = repository.GetById("Person", id).Result;
            Assert.True(person.Count() == 1);
        }

        [Fact]
        public void VerifyExistFunction()
        {
            var id = new Guid("00000000000000000000000000000001");
            Assert.True(repository.Exist("Person", id).Result[0]);
        }

        [Fact]
        public void VerifyReplacementValues()
        {
            var id = new Guid("00000000000000000000000000000001");
            Assert.True(repository.GetById("Person", id).Result[0].Name == "Jane Buck");
        }
    }
}
