using System.ComponentModel.DataAnnotations;
using MimeKit;

namespace Domain.DTOs.AccountDTOs;

public class ForgotPasswordDto
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
}