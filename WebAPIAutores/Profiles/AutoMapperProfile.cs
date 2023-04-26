using AutoMapper;
using WebAPIAutores.Dtos;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateAuthorDto, Author>();
        CreateMap<Author, GetAuthorDto>();
        CreateMap<CreateBookDto, Book>()
            .ForMember(x => x.AuthorsBooks, opts => opts.MapFrom(MapAuthorsBooks));
        CreateMap<Book, GetBookDto>();
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, GetCommentDto>();
    }

    private List<AuthorBook> MapAuthorsBooks(CreateBookDto createBookDto, Book book)
    {
        var result = new List<AuthorBook>();
        if (createBookDto.AuthorsIds is null) return result;

        foreach(var authorId in createBookDto.AuthorsIds)
        {
            result.Add(new AuthorBook { AuthorId = authorId });
        }

        return result;
    }
}
