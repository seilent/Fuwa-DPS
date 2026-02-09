using BPSR_FDPServ.Models;
using System.Text.Json.Serialization;

namespace BPSR_FDPServ
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(HttpResponseMessage))]
    [JsonSerializable(typeof(Settings))]
    [JsonSerializable(typeof(DedupeResp))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}
