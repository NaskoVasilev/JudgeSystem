using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

using Microsoft.AspNetCore.Identity;

namespace JudgeSystem.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
			Submissions = new HashSet<Submission>();
			UserContests = new HashSet<UserContest>();
        }

		[MaxLength(ModelConstants.UserFirstNameMaxLength)]
		[Required]
		public string Name { get; set; }

		[MaxLength(ModelConstants.UserSurnameMaxLength)]
		[Required]
		public string Surname { get; set; }

		public string StudentId { get; set; }
		public Student Student { get; set; }

		// Audit info
		public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

		public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

		public virtual ICollection<Submission> Submissions { get; set; }

		public virtual ICollection<UserContest> UserContests { get; set; }

        public virtual ICollection<UserPractice> UserPractices { get; set; }
    }
}
