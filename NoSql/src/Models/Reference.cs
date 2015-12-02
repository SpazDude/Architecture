using System;

namespace Models
{
    public class Reference<T> where T : AbstractBaseClass
    {
        public Guid RefId { get; set; }
    }
}
