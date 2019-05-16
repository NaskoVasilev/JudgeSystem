namespace JudgeSystem.Web.ViewModels.Settings
{
    using JudgeSystem.Data.Models;
    using JudgeSystem.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
