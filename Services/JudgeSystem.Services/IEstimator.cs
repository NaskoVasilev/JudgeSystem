namespace JudgeSystem.Services
{
	public interface IEstimator
	{
		int CalculteProblemPoints(int testsCount, int passedTests, int maxPoints);
	}
}
