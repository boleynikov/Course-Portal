namespace Services
{
    public class Validator
    {
        public CourseValidator Course;
        public MaterialsValidator Material;
        public Validator()
        {
            Course = new CourseValidator();
            Material = new MaterialsValidator();
        }
    }
}
