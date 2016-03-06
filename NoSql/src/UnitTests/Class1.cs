using NoSql.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {
        [Fact]
        public void UniTest1()
        {
            var repository = new MemoryRepository();
            
            Assert.True(true);
        }
    }
}
