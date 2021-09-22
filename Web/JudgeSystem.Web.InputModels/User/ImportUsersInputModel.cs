using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.User
{
    public class ImportUsersInputModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
