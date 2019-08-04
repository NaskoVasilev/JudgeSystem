namespace JudgeSystem.Common
{
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Administrator";

        public const string StudentRoleName = "Student";

        public const string BaseRoleName = "User";

        public const string AdministrationArea = "Administration";

		public const string InfoKey = "info";

		public const string ErrorKey = "error";

		public const string FileStorePath = "F:\\JudgeSystemStore\\";

        public const string OctetStreamMimeType = "application/octet-stream";

        public const int SessionIdleTimeout = 2;

		public const string StandardDateFormat = "dd/MM/yyyy HH:mm:ss";

		public const string StudentProfileActivationEmailSubject = "Student profile activation";

		public const string TemplatesFolder = "Templates";

		public const string EmailTemplatesFolder = "EmailTemplates";

		public const int PaginationOffset = 5;

		public const int SearchKeywordMinLength = 3;

		public const int MinClassNumber = 8;

		public const int MaxClassNumber = 12;

		public const int MinStudentsInClass = 1;

		public const int MaxStudentsInClass = 26;

        public const int SubmissionFileMaxSizeInKb = 16;

        public const int MinSubmissionCodeLength = 10;

        public const int MaxSubmissionCodeLength = 15000;

        public const string Location = "Location";

        public const string InvalidPracticeId = "Practice id must be specified";

        public const int SubmissionsPerPage = 5;

        public const int RecommendedLessonsCachingMinutes = 60;

        public const string EmailConfirmMessage = "Check your email for confirmation.";

        public const string LessonsRrecommendationMlModelPath = "MLModels/JudgeSystemLessonsModel.zip";

        public const double DefaultAllowedMemoryInMegaBytes = 30;

        public const int DefaultAllowedTimeInMilliseconds = 200;

        public const int MinAllowedMemoryInMegaBytes = 10;

        public const int MaxAllowedMemoryInMegaBytes = 100;

        public const int MinAllowedTimeInMilliseconds = 50;

        public const int MaxAllowedTimeInMilliseconds = 1000;

        public const int DefaultMaxPoints = 100;
    }
}
