namespace Models
{
    public class Course: AbstractBaseClass
    {
        public string CourseNumber { get; set; }
        public string Title { get; set; }
        public decimal Credits { get; set; }

        public Course() { }
    }
}
