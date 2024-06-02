using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}