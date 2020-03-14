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

        public const string InvalidLoginAttempt = "Invalid login attempt.";

        public const string ContestsAccessibleOnlyForStudents = "Contests are accessible only for students.";

        public static string UnsuportedZipSubmission = "{0} zip submission is unsupported.";

        public const string InvalidMainJavaClass = "Please provide valid class with main method to your solution.";

        public const string EmptyArchiveSubmitted = "Submitted archive should contains at least one file.";

        public const string UserWithTheSameEmailAlreadyExists = "User with the same email already exists.";

        public const string StudentWithTheSameEmailAlreadyExists = "Student with the same email already exists.";

        public const string StudentWithTheSameNumberInClassExists = "Student with the same number exists in {0} class.";

        public const string UserNotFound = "User with provided credentials was not found!";

        public const string InvalidFileExtension = "Your file must be in {0} format. Its extension should be {0}";

        public const string InvalidTest = "Test #{0} is invalid.";

        public const string InvalidJsonFile = "Your json file is not valid. See template for more information about file format.";

        public const string NotConfirmedEmail = "Your email is not confirmed. Please confirm your email before login!";

        public const string ContestIsNotActive = "This contest is not active.";

        public const string SendSubmissionToEarly = "Please, wait {0} seconds before send your submission!";

        public const string InvalidIpAddress = "Please, provide valid IPv6 or IPv4";

        public const string DeniedAccessToContestByIp = "You are not allowed to send submissions for this contest from ip: {0}";

        public const string SimilarSubmission = "Your code is very similar to another submission in the system";
    }
}
