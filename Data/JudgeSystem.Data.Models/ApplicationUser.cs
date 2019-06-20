namespace JudgeSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using JudgeSystem.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
			this.Submissions = new List<Submission>();
			this.UserContests = new HashSet<UserContest>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

		public string StudentId { get; set; }
		public Student Student { get; set; }

		public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

		public virtual ICollection<Submission> Submissions { get; set; }

		public virtual ICollection<UserContest> UserContests { get; set; }
	}
}
