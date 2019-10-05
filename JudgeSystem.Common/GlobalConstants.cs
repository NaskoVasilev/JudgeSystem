using System;
using System.Globalization;

namespace JudgeSystem.Common
{
    public static class GlobalConstants
    {
        #region Application Constants
        public const string DefaultConnectionStringName = "DefaultConnection";
        public const string AdministratorRoleName = "Administrator";
        public const string StudentRoleName = "Student";
        public const string BaseRoleName = "User";
        public const string AdministrationArea = "Administration";
        public const string LessonsRrecommendationMlModelPath = "MLModels/JudgeSystemLessonsModel.zip";
        public const string CurrentCultureInfo = "bg-BG";
        public static string DefaultDateTimeFormat = CultureInfo.GetCultureInfo(CurrentCultureInfo).DateTimeFormat.FullDateTimePattern;
        #endregion

        #region Keys
        public const string InfoKey = "info";
        public const string ErrorKey = "error";
        #endregion

        #region Folder names, mime types, formats, file names
        public const string OctetStreamMimeType = "application/octet-stream";
        public const string StandardDateFormat = "dd/MM/yyyy HH:mm:ss";
        public const string TemplatesFolder = "Templates";
        public const string Location = "Location";
        public const string EmailTemplatesFolder = "EmailTemplates";
        public const string JsonContentType = "text/json";
        public const string PersonalDataFileName = "JudgePersonalData.json";
        public static string[] AllowedFileExtensoins = new string[] { ".ppt", ".pptx", ".doc", ".docx", ".xls", ".cs", ".zip", ".json", ".xml", ".mp4", ".avi", ".txt", ".html", ".pdf" };
        #endregion

        #region Student and class related constants
        public const int MinClassNumber = 8;
        public const int MaxClassNumber = 12;
        public const int MinStudentsInClass = 1;
        public const int MaxStudentsInClass = 26;
        #endregion

        #region Pagination constants
        public const int PaginationOffset = 5;
        public const int SubmissionsPerPage = 5;
        public const string QueryStringDelimiter = "&";
        public const string ProblemIdKey = "problemId";
        public const string PageKey = "page";
        public const int ContestsPerPage = 10;
        public const int FeedbacksPerPage = 6;
        public const int StudentsPerPage = 9;
        #endregion

        #region Email constants
        public const string StudentProfileActivationEmailSubject = "Student profile activation";
        public const string ConfirmEmailSubject = "Confirm your email";
        public static string EmailConfirmationMessage = "Please confirm your account by <a href='{0}'>clicking here</a>.";
        public const string ResetPasswordEmailSubject = "Reset Password";
        public const string PasswordResetMessage = "Please reset your password by <a href='{0}'>clicking here</a>.";
        #endregion

        #region Submissions and problems related constants
        public static string CompilationDirectoryPath = $"{Environment.CurrentDirectory}/";
        public const string CSharpFileExtension = ".cs";
        public const string JavaFileExtension = ".java";
        public const string CppFileExtension = ".cpp";
        public const int SubmissionFileMaxSizeInKb = 16;
        public const int MinSubmissionCodeLength = 10;
        public const int MaxSubmissionCodeLength = 10000;
        public const double DefaultAllowedMemoryInMegaBytes = 50;
        public const int DefaultAllowedTimeInMilliseconds = 1500;
        public const int MinAllowedMemoryInMegaBytes = 10;
        public const int MaxAllowedMemoryInMegaBytes = 200;
        public const int MinAllowedTimeInMilliseconds = 50;
        public const int MaxAllowedTimeInMilliseconds = 5000;
        public const int DefaultMaxPoints = 100;
        #endregion

        #region Other numeric constants
        public const int RecommendedLessonsCachingMinutes = 60;
        #endregion
    }
}
