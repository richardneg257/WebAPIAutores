﻿namespace WebAPIAutores.Dtos;

public class GetBookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<GetAuthorDto> Authors { get; set; }
}
