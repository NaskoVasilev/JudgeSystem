namespace JudgeSystem.Services.Messaging
{
    public class BaseEmailOptions
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Fullname => $"{this.Name} {this.Surname}";
    }
}
