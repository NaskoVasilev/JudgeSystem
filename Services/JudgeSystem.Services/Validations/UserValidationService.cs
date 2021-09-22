using JudgeSystem.Common.Models;
using System.Collections.Generic;

using JudgeSystem.Services.Models.Users;
using JudgeSystem.Services.Validations.Contracts;
using System.Linq;

namespace JudgeSystem.Services.Validations
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IEmailValidationService emailValidator;

        public UserValidationService(IEmailValidationService emailValidator)
        {
            this.emailValidator = emailValidator;
        }

        public Result ValidateUsersForImport(IEnumerable<UserImportServiceModel> users)
        {
            if (users == null)
            {
                return Result.Failure("The Json file is invalid or empty! Please, provide valid file.");
            }

            var result = Result.Success();
            int index = 0;
            foreach (UserImportServiceModel user in users)
            {
                index++;
                if (!emailValidator.IsValid(user.Email))
                {
                    result.Errors.Add($"User {index} has invalid email! The invalid value is: {user.Email}");
                }
            }

            return result;
        }
    }
}
