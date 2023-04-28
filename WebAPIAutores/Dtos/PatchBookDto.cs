using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Dtos;

public class PatchBookDto
{
    [Required]
    [FirstCapitalLetter]
    [StringLength(250)]
    public string Title { get; set; }
    public DateTime PublicationDate { get; set; }
}
