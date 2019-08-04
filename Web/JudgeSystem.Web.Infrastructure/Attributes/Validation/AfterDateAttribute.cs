using System;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.Infrastructure.Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class AfterDateAttribute : ValidationAttribute
	{
		private readonly string errorMessage;
		private readonly string targetProperyName;

		public AfterDateAttribute(string targetProperyName, string errorMessage) : base(errorMessage)
		{
			this.targetProperyName = targetProperyName;
			this.errorMessage = errorMessage;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			DateTime targetProperyValue = (DateTime)validationContext.ObjectType.GetProperty(targetProperyName)
				.GetValue(validationContext.ObjectInstance);

			if((DateTime)value > targetProperyValue)
			{
				return ValidationResult.Success;
			}
			return new ValidationResult(this.errorMessage);
		}
	}
}
