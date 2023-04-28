namespace WebAPIAutores.Dtos;

public class GetBookDtoWithAuthors : GetBookDto
{
    public List<GetAuthorDto> Authors { get; set; }
}
