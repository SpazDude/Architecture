using NoSql.Repositories;
using System;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class MemoryRepsitoryTests
    {
        IRepository<dynamic> repository = new DynamicMemoryRepository(new TestIdFactory());
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
            var results = repository.Exist("Person", id).Result;
            Assert.True(results.All(x => x));
        }

        [Fact]
        public void VerifyReplacementValues()
        {
            var id = new Guid("00000000000000000000000000000001");
            var results = repository.GetById("Person", id).Result;
            Assert.True(results.All(x=> x.Name == "Jane Buck"));
        }
    }
}
