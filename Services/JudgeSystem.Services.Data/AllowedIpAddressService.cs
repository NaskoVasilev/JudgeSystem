using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.InputModels.AllowedIpAddress;
using JudgeSystem.Web.ViewModels.AllowedIpAddress;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class AllowedIpAddressService : IAllowedIpAddressService
    {
        private readonly IDeletableEntityRepository<AllowedIpAddress> repository;
        private readonly IRepository<AllowedIpAddressContest> allowedIpAddressContestRepository;

        public AllowedIpAddressService(
            IDeletableEntityRepository<AllowedIpAddress> repository,
            IRepository<AllowedIpAddressContest> allowedIpAddressContestRepository)
        {
            this.repository = repository;
            this.allowedIpAddressContestRepository = allowedIpAddressContestRepository;
        }

        public IEnumerable<AllowedIpAddressViewModel> All() =>
            repository.All()
            .To<AllowedIpAddressViewModel>()
            .ToList();

        public IEnumerable<AllowedIpAddressViewModel> ContestAllowedIpAddresses(int contestId) =>
            allowedIpAddressContestRepository.All()
            .Include(a => a.AllowedIpAddress)
            .Where(a => a.ContestId == contestId)
            .Select(a => new AllowedIpAddressViewModel 
            {
                Id = a.AllowedIpAddressId,
                Value = a.AllowedIpAddress.Value
            })
            .ToList();

        public async Task Create(AllowedIpAddressInputModel model) =>
            await repository.AddAsync(model.To<AllowedIpAddress>());

        public async Task Delete(int id)
        {
            var contestAllowedIpAddresses = allowedIpAddressContestRepository.All()
                .Where(ip => ip.AllowedIpAddressId == id)
                .ToList();
            await allowedIpAddressContestRepository.DeleteRangeAsync(contestAllowedIpAddresses);

            AllowedIpAddress allowedIpAddress = await repository.FindAsync(id);
            await repository.DeleteAsync(allowedIpAddress);

        }

        public T GetById<T>(int id) =>
            repository.All()
            .Where(a => a.Id == id)
            .To<T>()
            .FirstOrDefault();

        public IEnumerable<AllowedIpAddressViewModel> NotAddedIpAdresses(int contestId) => 
            repository.All()
                .Include(c => c.Contests)
                .Where(ip => !ip.Contests.Any(c => c.ContestId == contestId))
                .To<AllowedIpAddressViewModel>()
                .ToList();
    }
}
