﻿using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Dtos;

public class CreateBookDto
{
    [Required]
    [FirstCapitalLetter]
    [StringLength(250)]
    public string Title { get; set; }
    public DateTime PublicationDate { get; set; }
    public List<int> AuthorsIds { get; set; }
}
