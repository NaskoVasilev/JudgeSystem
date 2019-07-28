namespace JudgeSystem.Services
{
    public interface IPaginationHelper
    {
        int CalculatePagesCount(int elementsCount, int elementsPerPage);
    }
}
