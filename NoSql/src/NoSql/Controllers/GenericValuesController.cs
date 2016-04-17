using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using NoSql.Repositories;
using System.Collections.Generic;
using Models;

namespace NoSql.Controllers
{
    [Route("api/{resource}")]
    public class GenericValuesController<T> : Controller where T : IId
    {
        private IRepository<T> _repository;

        public GenericValuesController(IRepository<T> repository)
        {
            _repository = repository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<T>> Get(string resource)
        {
            return await _repository.GetAll(resource);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<T>> Get(string resource, params Guid[] id)
        {
            return await _repository.GetById(resource, id);
        }

        // POST api/valuesS
        [HttpPost]
        public void Post(string resource, [FromBody]T value)
        {
            _repository.Create(resource, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string resource, Guid id, [FromBody]T value)
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
