using System;

namespace Models
{
    public abstract class AbstractBaseClass: IId
    {
        public Guid Id { get; set; }
    }
}
