using System.Threading.Tasks;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Services.Data
{
	public interface IContestService
	{
		Task Create(Contest contest);
	}
}
