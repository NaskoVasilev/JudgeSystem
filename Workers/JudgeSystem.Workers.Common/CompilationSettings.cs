using JudgeSystem.Common;
using System;
using System.Runtime.InteropServices;

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

        private const string UnsupportedOSErrorMessage = "Unsupported OS. Commands cannot be run on currently used OS";

        public static string ConsoleFile
        {
            get
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "cmd.exe";
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "/bin/bash";
                }
                else
                {
                    throw new ArgumentException(UnsupportedOSErrorMessage);
                }
            }
        }

        public static string ConsoleComamndPrefix
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "-c";
                }
                else
                {
                    return "/c";
                }
            }
        }
    }
}
