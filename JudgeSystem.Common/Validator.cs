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

        public static void ThrowEntityNotFoundExceptionIfEntityIsNull(object entity)
        {
            ThrowEntityNotFoundExceptionIfEntityIsNull(entity, entity.GetType().Name);
        }
    }
}
