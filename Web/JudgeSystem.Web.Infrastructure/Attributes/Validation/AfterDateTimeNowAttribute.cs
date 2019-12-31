using System;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.Infrastructure.Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class AfterDateTimeNowAttribute : ValidationAttribute
	{
		public const string DefaultMessage = "The date must be in the future.";

		public AfterDateTimeNowAttribute() : base(DefaultMessage)
		{
		}

		public AfterDateTimeNowAttribute(string errorMessage) : base(errorMessage)
		{
		}

        public override bool IsValid(object value) => ((DateTime)value) >= DateTime.Now;
    }
}
