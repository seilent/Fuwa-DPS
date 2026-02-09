namespace BPSR_FDPServ.Models
{
    public class Settings
    {
        public bool EnableDiscordWebhookProxy { get; set; } = true;
        public bool EnableReportDeduplicationAPI { get; set; } = true;
        public TimeSpan DupeWindowDuration { get; set; } = TimeSpan.FromSeconds(10);
        public int DedupeListCleanUpAfterXEntries { get; set; } = 100;
    }
}
