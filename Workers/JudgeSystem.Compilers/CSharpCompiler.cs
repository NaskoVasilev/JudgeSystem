using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using JudgeSystem.Workers.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JudgeSystem.Compilers
{
    public class CSharpCompiler : ICompiler
	{
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources)
		{
			var syntaxTrees = new List<SyntaxTree>();
			foreach (string sourceCode in sources)
			{
				SyntaxTree syntaxTree = SyntaxFactory.ParseSyntaxTree(sourceCode);
				syntaxTrees.Add(syntaxTree);
			}
			CSharpCompilation compilation = BuildCSharpCompilation(fileName, syntaxTrees);

            string outputDllPath = GetOutputDllPath(fileName, workingDirectory);
			EmitResult emitResult = compilation.Emit(outputDllPath);

			if(!emitResult.Success)
			{
				var errors = new List<string>();
				foreach (Diagnostic diagnsotic in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
				{
					errors.Add(diagnsotic.GetMessage());
				}

				return new CompileResult(string.Join(Environment.NewLine, errors));
			}

			string runtimeConfigJsonFileContent = GenerateRuntimeConfigJsonFile();
            string runtimeConfigJsonFilePath = GetRuntimeConfigJsonFilePath(fileName, workingDirectory);
			File.WriteAllText(runtimeConfigJsonFilePath, runtimeConfigJsonFileContent);

			return new CompileResult() { OutputFilePath = outputDllPath };
		}

        private string GetOutputDllPath(string fileName, string workingDirectory) => workingDirectory + fileName + CompilationSettings.CSharpOutputFileExtension;

        private string GetRuntimeConfigJsonFilePath(string fileName, string workingDirectory) =>  workingDirectory  + fileName + CompilationSettings.RunTimeConfigJsonFileName;

        private CSharpCompilation BuildCSharpCompilation(string fileName, List<SyntaxTree> syntaxTrees)
		{
			CSharpCompilation compilation = CSharpCompilation.Create(fileName)
						.WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
						.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
						.AddSyntaxTrees(syntaxTrees);

			var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard"));
			compilation = compilation.AddReferences(MetadataReference.CreateFromFile(netStandardAssembly.Location));
			AssemblyName[] netStandardAssemblies = netStandardAssembly.GetReferencedAssemblies();

			foreach (AssemblyName assembly in netStandardAssemblies)
			{
				string assemblyLocation = Assembly.Load(assembly).Location;
				compilation = compilation.AddReferences(MetadataReference.CreateFromFile(assemblyLocation));
			}

			return compilation;
		}

		private static string GenerateRuntimeConfigJsonFile()
		{
			var runtimeConfigJson = new
			{
				RuntimeOptions = new
				{
					Tfm = "netcoreapp2.2",
					Framework = new
					{
						Name = "Microsoft.NETCore.App",
						Version = "2.2.0"
					}
				}
			};

			string runtimeConfigJsonFileContent = JsonConvert.SerializeObject(runtimeConfigJson, new JsonSerializerSettings()
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new CamelCaseNamingStrategy()
				},
				Formatting = Formatting.Indented
			});

			return runtimeConfigJsonFileContent;
		}
    }
}
