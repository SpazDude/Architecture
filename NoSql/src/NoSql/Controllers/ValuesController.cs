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
        public async Task<dynamic[]> Get(string resource)
        {
            return await _repository.GetAll(resource);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<dynamic[]> Get(string resource, params Guid[] id)
        {
            return await _repository.GetById(resource, id);
        }

        // POST api/values
        [HttpPost]
        public void Post(string resource, [FromBody]dynamic value)
        {
            _repository.Create(resource, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string resource, Guid id, [FromBody]dynamic value)
        {
            _repository.Update(resource, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string resource, params Guid[] id)
        {
            _repository.Delete(resource, id);
        }
    }
}
