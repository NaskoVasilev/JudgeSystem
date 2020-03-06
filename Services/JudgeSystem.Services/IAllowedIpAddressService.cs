using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.InputModels.AllowedIpAddress;
using JudgeSystem.Web.ViewModels.AllowedIpAddress;

namespace JudgeSystem.Services
{
    public interface IAllowedIpAddressService
    {
        Task Create(AllowedIpAddressInputModel model);

        IEnumerable<AllowedIpAddressViewModel> All();

        IEnumerable<AllowedIpAddressViewModel> ContestAllowedIpAddresses(int contestId);

        Task Delete(int id);

        T GetById<T>(int id);
    }
}
