using System.ComponentModel;

namespace Services.Helper
{
    /// <summary>
    /// Console Input Keys
    /// </summary>
    public static class Command
    {
        public const string HomePage = "home";
        public const string UserPage = "user";
        public const string CoursePage = "course";

        //home page
        public const string LoginCommand = "1";
        public const string RegisterCommand = "2";
        public const string AddCourseCommand = "3";
        public const string OpenCourseCommand = "4";
        public const string LogoutCommand = "5";
        public const string BackCommand = "6";
        public const string ExitCommand = "0";
        //user page
        public const string CreateCourseCommand = "c";
        public const string DeleteCourseCommand = "d";
        public const string StopAddingCommand = "s";

        public const string ArticleInputCase = "a";
        public const string PublicationInputCase = "p";
        public const string VideoInputCase = "v";

        //course page
        public const string EditCommand = "7";

        public const string EditCourseName = "8";
        public const string EditCourseDescription = "9";
        public const string AddCourseMaterials = "10";
        public const string DeleteCourseMaterial = "11";
        public const string AddNewOrEditSkill = "12";
        public const string DeleteSkill = "13";

        //material page
        public const string NextMaterial = "n";
        public const string BackToCourse = "b";
    }
}
