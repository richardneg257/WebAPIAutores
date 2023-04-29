using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Dtos;

public class EditAdminDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
