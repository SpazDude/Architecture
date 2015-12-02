using System.Collections.Generic;

namespace Models
{
    public class Section : AbstractBaseClass
    {
        public int Year { get; set; }
        public Semester Semester { get; set; }
        public Reference<Course> Course { get; set; }
        public List<Reference<Person>> Instructors { get; set; }
        public List<Reference<Person>> Students { get; set; }
    }
}
