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
	public class CSharpCompiler
	{
		private string assemblyName;

		public CSharpCompiler()
		{
            assemblyName = Guid.NewGuid().ToString();
		}

        public CompileResult CreateAssembly(string sourceCode) => CreateAssembly(new List<string>() { sourceCode });

        public CompileResult CreateAssembly(List<string> sourceCodeFiles)
		{
			var syntaxTrees = new List<SyntaxTree>();
			foreach (var sourceCode in sourceCodeFiles)
			{
				SyntaxTree syntaxTree = SyntaxFactory.ParseSyntaxTree(sourceCode);
				syntaxTrees.Add(syntaxTree);
			}
			CSharpCompilation compilation = BuildCSharpCompilation(syntaxTrees);

            string outputDllPath = GetOutputDllPath();
			EmitResult emitResult = compilation.Emit(outputDllPath);

			if(!emitResult.Success)
			{
				var errors = new List<string>();
				foreach (Diagnostic diagnsotic in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
				{
					errors.Add(diagnsotic.GetMessage());
				}

				return new CompileResult(errors);
			}

			string runtimeConfigJsonFileContent = GenerateRuntimeConfigJsonFile();
            string runtimeConfigJsonFilePath = GetRuntimeConfigJsonFilePath();
			File.WriteAllText(runtimeConfigJsonFilePath, runtimeConfigJsonFileContent);

			return new CompileResult(outputDllPath);
		}

        public void DeleteGeneratedFiles()
        {
            string outputDllPath = GetOutputDllPath();
            File.Delete(outputDllPath);
            string runtimeConfigJsonFilePath = GetRuntimeConfigJsonFilePath();
            File.Delete(runtimeConfigJsonFilePath);
        }

        private string GetOutputDllPath() => CompilationSettings.WorkingDirectoryPath + assemblyName + ".dll";

        private string GetRuntimeConfigJsonFilePath() => CompilationSettings.WorkingDirectoryPath + assemblyName + CompilationSettings.RunTimeConfigJsonFileName;


        private CSharpCompilation BuildCSharpCompilation(List<SyntaxTree> syntaxTrees)
		{
			CSharpCompilation compilation = CSharpCompilation.Create(assemblyName)
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
