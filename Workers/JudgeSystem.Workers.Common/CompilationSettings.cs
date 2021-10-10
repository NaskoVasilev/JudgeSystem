namespace JudgeSystem.Workers.Common
{
    public static class CompilationSettings
    {
        public const string RunTimeConfigJsonFileName = ".runtimeconfig.json";

        public const string CSharpOutputFileExtension = ".dll";

        public const string ExeFileExtension = ".exe";

        public const string JavaOutputFileExtension = ".jar";

        public static string CppCompilerPath { get; set; }

        public static string JavaCompilerPath { get; set; }

        public const string SetCompilerPathCommand = "set PATH=%PATH%;";

        public static string SetCPlusPlusCompilerPathCommand => GetCompilerPathCommand(CppCompilerPath);

        public static string SetJavaCompilerPathCommand => GetCompilerPathCommand(JavaCompilerPath);

        public static string GetCompilerPathCommand(string compilerPath)
        {
            if (!string.IsNullOrEmpty(compilerPath))
            {
                return $"{SetCompilerPathCommand}{compilerPath};";
            }

            return "";
        }
    }
}
