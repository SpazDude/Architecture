using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicApi.Web.Models
{
    public class Value : IId
    {
        public Value()
        {
            Name = this.GetType().FullName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
