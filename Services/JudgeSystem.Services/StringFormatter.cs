namespace JudgeSystem.Services
{
    public class StringFormatter : IStringFormatter
    {
        public string FormatPoints(int points, int maxPoints) => $"{points}/{maxPoints}";
    }
}
