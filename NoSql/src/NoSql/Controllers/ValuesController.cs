using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using NoSql.Repositories;

namespace NoSql.Controllers
{
    [Route("api/{resource}")]
    //[Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IRepository _repository;

        public ValuesController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get(string resource)
        {
            

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string resource, Guid id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post(string resource, [FromBody]string value)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string resource, Guid id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string resource, Guid id)
        {
        }
    }
}
