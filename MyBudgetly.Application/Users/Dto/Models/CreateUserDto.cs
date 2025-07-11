using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MyBudgetly.Application.Users.Dto.Models;

public class CreateUserDto
{
    [JsonProperty(Required = Required.Always)]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string? BackupEmail { get; set; }
}
