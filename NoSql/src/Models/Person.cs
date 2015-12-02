using System;

namespace Models
{
    public class Person: AbstractBaseClass
    {
        public string GivenNames{get; set;}
        public string FamilyNames{get; set;}
        public string Identifier{get; set;}
        public DateTime BirthDate{get; set;}
    }
}
