using BPSR_FDPServ.Models;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.IO.Hashing;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace BPSR_FDPServ
{
    public class DiscordWebHookManager(IOptions<Settings> settings) : DedupeManager(settings)
    {
        private readonly HttpClient HttpClient = new ();

        public async Task<HttpResponseMessage> ProcessEncounterReport(string id, string token, ulong teamId, string payload, IFormFileCollection files)
        {
            if (IsDupe(id, token, teamId))
            {
                return new HttpResponseMessage(HttpStatusCode.AlreadyReported);
            }

            var sendUrl = $"https://discord.com/api/webhooks/{id}/{token}";
            var result = await SendWebhook(sendUrl, payload, files);

            return result;
        }

        private bool IsDupe(string discordId, string discordToken, ulong teamId)
        {
            var id = CreateTeamHookReportId(discordId, discordToken, teamId);
            var isDupe = IsDupe(id);

            return isDupe;
        }

        private ulong CreateTeamHookReportId(string id, string token, ulong teamId)
        {
            var hash = new XxHash64();
            hash.Append(MemoryMarshal.Cast<ulong, byte>([teamId]));
            hash.Append(Encoding.UTF8.GetBytes(id));
            hash.Append(Encoding.UTF8.GetBytes(token));
            var hashUlong = hash.GetCurrentHashAsUInt64();

            return hashUlong;
        }

        private async Task<HttpResponseMessage> SendWebhook(string url, string payload, IFormFileCollection files)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                form.Add(new StringContent(payload, Encoding.UTF8, "application/json"), "payload_json");

                foreach (var file in files)
                {
                    var fileStream = file.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    form.Add(fileContent, file.Name, file.FileName);
                }

                var response = await HttpClient.PostAsync(url, form);

                return response;
            }
            catch (Exception ex)
            {

            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
