using System;

namespace JudgeSystem.Services
{
    public class PaginationHelper : IPaginationHelper
    {
        public int CalculatePagesCount(int elementsCount, int elementsPerPage)
        {
            return (int)Math.Ceiling((double)elementsCount / elementsPerPage);
        }
    }
}
