using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using DynamicApi.Web.Models;
using DynamicApi.Web.Repositories;

namespace DynamicApi.Web.Controllers
{
    //[Route("api/[controller]")]
    //public class ValuesController : Controller<Values>
    //{
    //    public ValuesController(IRepository<Values> repository) : base(repository) { }
    //}

    public class Controller<T> : Controller where T : IId
    {
        private IRepository<T> _repository;

        public Controller(IRepository<T> repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<T> Get()
        {
            return _repository.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]T value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]T value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
