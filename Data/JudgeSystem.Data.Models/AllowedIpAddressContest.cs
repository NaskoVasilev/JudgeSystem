namespace JudgeSystem.Data.Models
{
    public class AllowedIpAddressContest
    {
        public int AllowedIpAddressId { get; set; }
        public AllowedIpAddress AllowedIpAddress { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }
    }
}
