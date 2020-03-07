namespace JudgeSystem.Common
{
    public static class ModelConstants
    {
        #region User models constants
        public const int UserFirstNameMaxLength = 30;
        public const int UserFirstNameMinLength = 2;
        public const int UserSurnameMaxLength = 30;
        public const int UserSurnameMinLength = 3;
        public const int UserUsernameMinLength = 3;
        public const int UserUsernameMaxLength = 30;
        public const int UserPasswordMinLength = 5;
        public const int UserPasswordMaxLength = 50;
        public const string UserNewPasswordDisplayName = "New password";
        public const string UserOldPasswordDisplayName = "Current password";
        public const string UserConfirmPasswordDisplayName = "Confirm password";
        public const string UserPhoneNumberDisplayName = "Phone number";
        public const string UserLoginRememeberMeDisplayName = "Remember me?";
        #endregion

        #region Contest models consatnts
        public const int ContestNameMaxLength = 50;
        public const int ContestNameMinLength = 5;
        public const string ContestStartTimeDisplayName = "Start time";
        public const string ContestEndTimeDisplayName = "End time";
        public const string ContestLessonIdDisplayName = "Lesson";
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
        public const int ProblemNameMinLength = 3;
        public const int ProblemMinPoints = 1;
        public const int ProblemMaxPoints = 300;
        public const int ProblemMinTimeIntervalBetweenSubmissionInSeconds = 5;
        public const int ProblemMaxTimeIntervalBetweenSubmissionInSeconds = 3000;
        public const string ProblemIsExtraTaskDisplayName = "Extra task";
        public const string ProblemMaxPointsDisplayName = "Max points";
        public const string ProblemAllowedTimeInMillisecondsDisplayName = "Allowed time in miliseconds";
        public const string ProblemAllowedMemoryInMegaBytesDisplayName = "Allowed memory in MB";
        public const string ProblemSubmissionTypeDisplayName = "Submission type";
        public const string ProblemTestingStrategyDisplayName = "Testing type";
        public const string ProblemTimeIntervalBetweenSubmissionInSecondsDisplayName = "Time interval between submission in seconds";
        public const string ProblemCheckOutputStrategyDisplayName = "Compile, execute and check produced output";
        public const string ProblemRunAutomatedTestsStrategyDisplayName = "Run automated tests to check submission";
        #endregion

        #region Resource models consatnts
        public const int ResourceNameMaxLength = 30;
        public const int ResourceNameMinLength = 3;
        #endregion

        #region Student models consatnts
        public const int StudentFullNameMaxLength = 50;
        public const int StudentFullNameMinLength = 10;
        public const int StudentEmailMaxLength = 30;
        public const string StudentActivationKeyDisplayName = "Activation key";
        public const string StudentFullNameDisplayName = "Full Name";
        public const string StudentSchoolClassIdDisplayName = "Grade";
        public const string StudentNumberInClassDisplayName = "Number in class";

        #endregion

        #region Test models consatnts
        public const int TestInputDataMaxLength = 2000000000;
        public const int TestOutputDataMaxLength = 2000000000;
        public const string TestInputDataDisplayName = "Input";
        public const string TestOutputDataDisplayName = "Expected output";
        public const string TestIsTrialTestDisplayName = "Trial test";
        public const string TestJsonImportStrategyDisplayName = "Json file with all the data";
        public const string TestZipImportStrategyDisplayName = "Zip file with files per input and output";
        public const string TestingProjectStrategyDisplayName = "Testing project with automated tests";
        #endregion

        #region Feedback models constants
        public const int FeedbackContentMaxLength = 1500;
        public const int FeedbackContentMinLength = 5;
        #endregion
    }
}
