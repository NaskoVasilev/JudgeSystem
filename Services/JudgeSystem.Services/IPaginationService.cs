namespace JudgeSystem.Services
{
    public interface IPaginationService
    {
        int CalculatePagesCount(int elementsCount, int elementsPerPage);
    }
}
