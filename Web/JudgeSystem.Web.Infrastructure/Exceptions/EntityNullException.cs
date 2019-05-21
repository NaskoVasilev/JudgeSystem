using JudgeSystem.Common;
using System;

namespace JudgeSystem.Web.Infrastructure.Exceptions
{
	public class EntityNullException : ArgumentException
	{
		private const string DefaultMessage = "Invalid entity id.";

		public EntityNullException() : base(DefaultMessage)
		{
		}

		public EntityNullException(string entityName) : base(string.Format(ErrorMessages.NotFoundEntityMessage, entityName))
		{
		}

		public EntityNullException(string message, string entityName) : base(message, entityName)
		{
		}
	}
}
