using BPSR_FDPServ.Models;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace BPSR_FDPServ
{
    public class DedupeManager(IOptions<Settings> settings)
    {
        private ConcurrentDictionary<ulong, ReportTeamState> ReportDedupeData = [];

        public bool IsDupe(ulong teamId)
        {
            var report = ReportDedupeData.AddOrUpdate(teamId,
                _ => new ReportTeamState(DateTime.Now),
                (key, value) =>
                {
                    value.CanReport = false;

                    if ((value.ReportedAt + settings.Value.DupeWindowDuration) <= DateTime.Now)
                    {
                        value.ReportedAt = DateTime.Now;
                        value.CanReport = true;
                    }

                    return value;
                });

            CheckAndCleanUpExpiredEntries();

            return !report.CanReport;
        }

        private void CheckAndCleanUpExpiredEntries()
        {
            if (ReportDedupeData.Count >= settings.Value.DedupeListCleanUpAfterXEntries)
            {
                foreach (var kvp in ReportDedupeData)
                {
                    if ((kvp.Value.ReportedAt + settings.Value.DupeWindowDuration) <= DateTime.Now)
                    {
                        ReportDedupeData.TryRemove(kvp);
                    }
                }
            }
        }
    }
}
