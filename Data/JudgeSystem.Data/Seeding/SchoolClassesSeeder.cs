namespace JudgeSystem.Data.Seeding
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Data;

	using Microsoft.Extensions.DependencyInjection;


	public class SchoolClassesSeeder : ISeeder
	{
		private const int MinClassNumber = 8;
		private const int MaxClassNumber = 12;


		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
		{
			ISchoolClassService schoolClassService = serviceProvider.GetRequiredService<ISchoolClassService>();
			IEnumerable<SchoolClassType> classTypes = Enum.GetValues(typeof(SchoolClassType)).Cast<SchoolClassType>();

			for (int classNumber = MinClassNumber; classNumber <= MaxClassNumber; classNumber++)
			{
				foreach (var classType in classTypes)
				{
					await SeedSchoolClass(classNumber, classType, schoolClassService);
				}
			}
		}

		private async Task SeedSchoolClass(int classNumber, SchoolClassType classType, ISchoolClassService schoolClassService)
		{
			bool exists = schoolClassService.ClassExists(classNumber, classType);
			if(!exists)
			{
		 		await schoolClassService.Create(classNumber, classType);
			}
		}
	}
}
