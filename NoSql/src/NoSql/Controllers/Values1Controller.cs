using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using NoSql.Repositories;

namespace NoSql.Controllers
{
    [Route("api1/{resource}")]
    public class Values1Controller : Controller
    {
        private IRepository<JObject> _repository;

        public Values1Controller(IRepository<JObject> repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<JObject[]> Get(string resource)
        {
            return await _repository.GetAll(resource);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<JObject[]> Get(string resource, Guid id)
        {
            return await _repository.GetById(resource, id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]JObject value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]JObject value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
