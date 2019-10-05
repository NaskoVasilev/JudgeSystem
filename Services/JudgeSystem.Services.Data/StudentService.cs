using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.Dtos.SchoolClass;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;
using JudgeSystem.Common;
using JudgeSystem.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class StudentService : IStudentService
	{
		private readonly IPasswordHashService passwordHashService;
		private readonly IRepository<Student> repository;

		public StudentService(IPasswordHashService passwordHashService, IRepository<Student> repository)
		{
			this.passwordHashService = passwordHashService;
			this.repository = repository;
		}

		public async Task<StudentDto> Create(StudentCreateInputModel model, string activationKey)
		{
            Student student = model.To<Student>();
			student.ActivationKeyHash = passwordHashService.HashPassword(activationKey);
			await repository.AddAsync(student);
			return student.To<StudentDto>();
		}

		public async Task<StudentDto> Delete(string id)
		{
            Student student = await repository.FindAsync(id);
            await repository.DeleteAsync(student);
            return student.To<StudentDto>();
		}

        public bool ExistsByEmail(string email) => 
            repository.All().Any(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

        public async Task<T> GetById<T>(string id)
		{
            T student = await repository.All().Where(s => s.Id == id).To<T>().FirstOrDefaultAsync();
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(student, nameof(Student));
			return student;
		}

		public async Task<SchoolClassDto> GetStudentClass(string id)
		{
			Student student = await repository.All()
                .Include(s => s.SchoolClass)
                .FirstOrDefaultAsync(s => s.Id == id);

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(student, nameof(Student));

            return student.SchoolClass.To<SchoolClassDto>();
		}

		public Task<StudentDto> GetStudentProfileByActivationKey(string activationKey)
		{
			string activationKeyHash = passwordHashService.HashPassword(activationKey);
			return repository.All()
                .Where(x => x.ActivationKeyHash == activationKeyHash)
                .To<StudentDto>()
                .FirstOrDefaultAsync();
		}

        public IEnumerable<StudentProfileViewModel> SearchStudentsByClass(int? classNumber, SchoolClassType? classType, int page, int studentsPerPage)
        {
            var students = FilterStudentsByClass(classNumber, classType)
                .OrderBy(s => s.SchoolClass.ClassNumber)
                .ThenBy(s => s.SchoolClass.ClassType)
                .ThenBy(s => s.NumberInCalss)
                .GetPage(page, studentsPerPage)
                .To<StudentProfileViewModel>()
                .ToList();

            return students;
        }

        public async Task SetStudentProfileAsActivated(string studentId)
		{
            Student student = await repository.FindAsync(studentId);
            student.IsActivated = true;
			await repository.UpdateAsync(student);
		}

        public int StudentsByClassCount(int? classNumber, SchoolClassType? classType) => FilterStudentsByClass(classNumber, classType).Count();

        public async Task<StudentDto> Update(StudentEditInputModel model)
		{
            Student student = await repository.FindAsync(model.Id);

            student.FullName = model.FullName;
			student.Email = model.Email;
			student.NumberInCalss = model.NumberInCalss;
			student.SchoolClassId = model.SchoolClassId;

			await repository.UpdateAsync(student);
			return student.To<StudentDto>();
		}

        public bool ExistsByClassAndNumber(int schoolClassId, int numberInCalss) => 
            repository.All().Any(x => x.SchoolClassId == schoolClassId && x.NumberInCalss == numberInCalss);

        private IQueryable<Student> FilterStudentsByClass(int? classNumber, SchoolClassType? classType)
        {
            IQueryable<Student> students = null;

            if (classNumber.HasValue && classType.HasValue)
            {
                students = repository.All()
                    .Where(s => s.SchoolClass.ClassNumber == classNumber.Value && s.SchoolClass.ClassType == classType.Value);
            }
            else if (classNumber.HasValue)
            {
                students = repository.All()
                    .Where(s => s.SchoolClass.ClassNumber == classNumber.Value);
            }
            else if (classType.HasValue)
            {
                students = repository.All()
                    .Where(s => s.SchoolClass.ClassType == classType.Value);
            }
            else
            {
                return new List<Student>().AsQueryable();
            }

            return students;
        }
    }
}
