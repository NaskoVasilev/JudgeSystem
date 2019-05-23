using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Utilites
{
	public interface IFileManager
	{
		string GenerateFileName(IFormFile file);

		Task UploadFile(IFormFile file, string fileName);

		void DeleteFile(string fileName);
	}
}
