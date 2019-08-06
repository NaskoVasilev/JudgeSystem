namespace JudgeSystem.Common
{
    public static class ErrorMessages
	{
        public const string NotFoundEntityMessage = "The required {0} was not found!";

		public const string DiffrentLessonPasswords = "Invalid old password.";

		public const string InvalidPassword = "Invalid password.";

		public const string TooShorPasswordMessage = "The lesson password must be at least five symbols.";

		public const string LockedLesson = "The lesson already has password!";

		public const string NotValidEmail = "The provided email is not valid";

		public const string InvalidActivationKey = "Your activation key is invalid.";

		public const string ActivatedStudentProfile = "Student profile with this activation key is already activated.";

		public const string InvalidStudentProfile = "Your student profile is not activated! Connect to administrators for more info!";

        public const string InvalidSearchKeyword = "The provided keyword must be at least one symbol long";

        public const string InvalidSubmission = "Please enter valid submission";

        public static string TooBigSubmissionFile = $"The zip file must be smaller than {GlobalConstants.SubmissionFileMaxSizeInKb} KB.";

        public static string TooLongSubmissionCode = $"The submitted code is too long.";

        public const string EndpointErrorMessage = "Some error occurs.";

        public const string InvalidSubmissionSource = "Either practiceId or contestId must be provided";

        public const string UnsupportedFileFormat = "{0} is not supported file format.";

        public const string WrongFileFormat = "Some of the files have worng format. Check extensions and try again.";

        public const string InvalidPracticeId = "Practice id must be specified";
    }
}
