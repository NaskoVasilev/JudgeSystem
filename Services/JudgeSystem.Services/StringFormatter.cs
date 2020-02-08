namespace JudgeSystem.Services
{
    public class StringFormatter : IStringFormatter
    {
        public string FormatPoints(int actualPoints, int maxPoints) => $"{actualPoints}/{maxPoints}";
    }
}
