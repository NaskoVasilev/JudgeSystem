using System;

namespace JudgeSystem.Services
{
    public class PaginationService : IPaginationService
    {
        public int CalculatePagesCount(int elementsCount, int elementsPerPage)
        {
            if(elementsPerPage <= 0)
            {
                throw new ArgumentException("Elements per page cannot be negative or zero number.");
            }

            if(elementsCount < 0)
            {
                throw new ArgumentException("Elements count cannot be negative number.");
            }

            return (int)Math.Ceiling((double)elementsCount / elementsPerPage);
        }
    }
}
