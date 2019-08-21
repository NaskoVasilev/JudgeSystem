using JudgeSystem.Common.Exceptions;

namespace JudgeSystem.Common
{
    public static class Validator
    {
        public static void ThrowEntityNotFoundExceptionIfEntityIsNull(object entity, string name)
        { 
            if(entity == null)
            {
                throw new EntityNotFoundException(name);
            }
        }
    }
}
