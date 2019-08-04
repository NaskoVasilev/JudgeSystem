namespace JudgeSystem.Common
{
    public static class ModelConstants
    {
        #region User models constants
        public const int UserFirstNameMaxLength = 30;
        public const int UserSurnameMaxLength = 30;
        #endregion

        #region Contest models consatnts
        public const int ContestNameMaxLength = 50;
        public const int ContestNameMinLength = 5;
        #endregion

        #region Lesson models consatnts
        public const int LessonNameMaxLength = 50;
        public const int LessonNameMinLength = 5;
        public const int LessonPasswordMinLength = 5;
        public const int LessonPasswordMaxLength = 50;
        public const string LessonPasswordDisplayName = "Lesson password";
        public const string LessonOldPasswordDisplayName = "Old lesson password";
        #endregion

        #region Course models consatnts
        public const int CourseNameMaxLength = 50;
        public const int CourseNameMinLength = 3;
        #endregion

        #region Problem models consatnts
        public const int ProblemNameMaxLength = 30;
        #endregion

        #region Resource models consatnts
        public const int ResourceNameMaxLength = 30;
        #endregion

        #region Student models consatnts
        public const int StudentFullNameMaxLength = 50;
        public const int StudentEmailMaxLength = 30;
        #endregion
    }
}
