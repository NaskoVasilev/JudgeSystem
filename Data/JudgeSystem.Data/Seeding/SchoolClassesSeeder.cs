using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;

using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Data.Seeding
{
    public class SchoolClassesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                IEnumerable<SchoolClassType> classTypes = Enum.GetValues(typeof(SchoolClassType)).Cast<SchoolClassType>();

                for (int classNumber = GlobalConstants.MinClassNumber; classNumber <= GlobalConstants.MaxClassNumber; classNumber++)
                {
                    foreach (SchoolClassType classType in classTypes)
                    {
                        await SeedSchoolClass(classNumber, classType, context);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private async Task SeedSchoolClass(int classNumber, SchoolClassType classType, ApplicationDbContext context)
        {
            bool exists = context.SchoolClasses.Any(x => x.ClassNumber == classNumber && x.ClassType == classType);

            if (!exists)
            {
                var schoolClass = new SchoolClass
                {
                    ClassNumber = classNumber,
                    ClassType = classType,
                };

                await context.SchoolClasses.AddAsync(schoolClass);
            }
        }
    }
}
