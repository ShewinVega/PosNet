using System.Text.Json.Serialization;

namespace PosNet.Infrastructure.ProblemsDetail
{
    public sealed class ErrorDetail
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Code { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
    }
}
