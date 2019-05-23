namespace JudgeSystem.Web.Utilites
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Http;

	public interface IFileManager
	{
		string GenerateFileName(IFormFile file);

		Task UploadFile(IFormFile file, string fileName);

		void DeleteFile(string fileName);
	}
}
