using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AccountDTOs;

public class LoginDto
{
    public required string UserName { get; set; }
    [DataType(DataType.Password)] public required string Password { get; set; }
}