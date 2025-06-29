using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetly.Application.Users.Dto.Models;
public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BackupEmail { get; set; }
}