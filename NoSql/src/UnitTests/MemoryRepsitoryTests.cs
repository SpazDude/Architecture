using MongoDB.Bson;
using NoSql.Repositories;
using System;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class MemoryRepsitoryTests
    {
        IRepository repository = new MemoryRepository(new TestIdFactory());
        

        public MemoryRepsitoryTests()
        {
            dynamic John = new ExpandoObject();
            John.Name = "John Smith";
            dynamic Jane = new ExpandoObject();
            Jane.Name = "Jane Doe";
            dynamic Guy = new ExpandoObject();
            Guy.Name = "I will be deleted";

            repository.Create("Person", John, Jane, Guy).Wait();
        }

        [Fact]
        public void RetrievePeopleTest()
        {
            var people = repository.GetAll("Person").Result;
            Assert.True(people.Count() == 3);
        }

        [Fact]
        public void RetrievePersonByIdTest()
        {
            var id = ObjectId.GenerateNewId(1);
            var person = repository.GetById("Person", id).Result;
            Assert.True(person.Count() == 1);
        }

        [Fact]
        public void VerifyExistFunction()
        {
            var id = ObjectId.GenerateNewId(1);
            var results = repository.Exist("Person", id).Result;
            Assert.True(results.All(x => x));
        }

        [Fact]
        public void VerifyReplacementValues()
        {
            var id = ObjectId.GenerateNewId(1);
            var results = repository.GetById("Person", id).Result;
            Assert.True(results.All(x=> x.Name == "Jane Buck"));
        }
    }
}
