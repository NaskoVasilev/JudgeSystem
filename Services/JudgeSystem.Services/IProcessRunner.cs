﻿using System.Threading.Tasks;
 
using JudgeSystem.Services.Models;

namespace JudgeSystem.Services
{
    public interface IProcessRunner
    {
        Task<ProcessResult> Run(string arguments, string workingDirectory, int timeout = ProcessRunner.DefaultTimeout);

        string PrependChangeDirectoryCommand(string command, string directoryPath);
    }
}
