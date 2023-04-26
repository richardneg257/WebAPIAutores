using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Dtos;

public class CreateBookDto
{
    [FirstCapitalLetter]
    [StringLength(250)]
    public string Title { get; set; }
}
