namespace WebAPIAutores.Dtos;

public class GetAuthorDtoWithBooks : GetAuthorDto
{
    public List<GetBookDto> Books { get; set; }
}
