namespace JudgeSystem.Services
{
	public class Estimator : IEstimator
	{
		public int CalculteProblemPoints(int testsCount, int maxPoints)
		{
			double pointsPerTest = (double)maxPoints / testsCount;
			double actualPoints = testsCount * pointsPerTest;
			return (int)actualPoints;
		}
	}
}
