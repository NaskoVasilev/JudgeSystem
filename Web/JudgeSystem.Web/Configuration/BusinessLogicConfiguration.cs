using JudgeSystem.Compilers;
using JudgeSystem.Executors;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;

using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class BusinessLogicConfiguration
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ILessonService, LessonService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IProblemService, ProblemService>();
            services.AddTransient<ISubmissionService, SubmissionService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IContestService, ContestService>();
            services.AddTransient<IExecutedTestService, ExecutedTestService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ISchoolClassService, SchoolClassService>();
            services.AddTransient<IPracticeService, PracticeService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IAllowedIpAddressService, AllowedIpAddressService>();

            services.AddTransient<IEstimator, Estimator>();
            services.AddTransient<IPasswordHashService, PasswordHashService>();
            services.AddTransient<IPaginationService, PaginationService>();
            services.AddTransient<ILessonsRecommendationService, LessonsRecommendationService>();
            services.AddTransient<IAzureStorageService, AzureStorageService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IStudentProfileService, StudentProfileService>();
            services.AddTransient<IUtilityService, UtilityService>();
            services.AddTransient<IRouteBuilder, RouteBuilder>();
            services.AddTransient<IChecker, Checker>();
            services.AddTransient<IJsonUtiltyService, JsonUtiltyService>();
            services.AddTransient<IExcelFileGenerator, ExcelFileGenerator>();
            services.AddTransient<IStringFormatter, StringFormatter>();
            services.AddTransient<IFileSystemService, FileSystemService>();
            services.AddTransient<IProcessRunner, ProcessRunner>();
            services.AddTransient<ICodeCompareer, CodeCompareer>();

            services.AddTransient<ICompilerFactory, CompilerFactory>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();

            return services;
        }
    }
}
