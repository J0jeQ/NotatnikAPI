using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotatnikAPI.Models;

public class Note
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
