namespace JudgeSystem.Web.Utilites
{
	using System.IO;
	using System.Threading.Tasks;

	using JudgeSystem.Common;

	using Microsoft.AspNetCore.Http;

	public class FileManager : IFileManager
	{
		public void DeleteFile(string fileName)
		{
			string filePath = GetPath(fileName);
			File.Delete(filePath);
		}

		public string GenerateFileName(IFormFile file)
		{
			string fileOriginalName = file.FileName;
			var fileName = Path.GetRandomFileName() + fileOriginalName;
			return fileName;
		}

		public async Task UploadFile(IFormFile file, string fileName)
		{
			var filePath = GetPath(fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
		}

		private string GetPath(string fileName) => GlobalConstants.FileStorePath + fileName;
	}
}
