using MongoDB.Bson;
using NoSql.Repositories;
using System;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class MongoDbRepsitoryTests
    {
        MongoDbRepository repository = new MongoDbRepository(
            new TestIdFactory(), "mongodb://127.0.0.1", "Test");

        dynamic John = new ExpandoObject();
        dynamic Jane = new ExpandoObject();
        dynamic Guy = new ExpandoObject();
        
        public MongoDbRepsitoryTests()
        {
            repository.DeleteResources("Person");
            
            John.Name = "John Smith";
            Jane.Name = "Jane Doe";
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
            var id = John._id;
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
            Assert.True(results.All(x => x.Name == "Jane Buck"));
        }
    }
}
