using System.Text.Json.Serialization;

namespace MiniCommerce.Api.App.Models;

public class ErrorModel
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("detail")]
    public object? Detail { get; set; }
}