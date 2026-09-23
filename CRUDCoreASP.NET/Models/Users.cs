using System.Text.Json.Serialization;

namespace CRUDCoreASP.NET.Models
{
    public class Users
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("MiddleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("LastName")]
        public string? LastName { get; set; }
    }
}
