using System;

namespace JudgeSystem.Common.Exceptions
{
    public class EntityNotFoundException : ArgumentException
    {
        private const string DefaultMessage = "The required entity was not found.";

        public EntityNotFoundException() : base(DefaultMessage)
        {
        }

        public EntityNotFoundException(string entityName) : base(string.Format(ErrorMessages.NotFoundEntityMessage, entityName))
        {
        }

        public EntityNotFoundException(string message, string entityName) : base(message, entityName)
        {
        }
    }
}
