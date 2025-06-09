using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MyBudgetly.Application.Users.Dto.Models;

public class UserDto
{
    [JsonProperty(Required = Required.Always)]
    [Required]
    public Guid UserId { get; set; }

    [JsonProperty(Required = Required.Always)]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [Required]
    public string LastName { get; set; } = string.Empty;
}