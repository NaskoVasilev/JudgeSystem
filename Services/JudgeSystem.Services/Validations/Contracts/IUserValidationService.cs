using System.Collections.Generic;

using JudgeSystem.Common.Models;

using JudgeSystem.Services.Models.Users;

namespace JudgeSystem.Services.Validations.Contracts
{
    public interface IUserValidationService
    {
        Result ValidateUsersForImport(IEnumerable<UserImportServiceModel> users);
    }
}
