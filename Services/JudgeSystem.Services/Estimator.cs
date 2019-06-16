namespace JudgeSystem.Services
{
	public class Estimator : IEstimator
	{
		public int CalculteProblemPoints(int testsCount, int passedTests, int maxPoints)
		{
			double pointsPerTest = (double)maxPoints / testsCount;
			double actualPoints = passedTests * pointsPerTest;
			return (int)actualPoints;
		}
	}
}
