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
        CreateMap<Author, GetAuthorDtoWithBooks>()
            .ForMember(x => x.Books, opts => opts.MapFrom(MapGetAuthorDtoBooks));
        CreateMap<CreateBookDto, Book>()
            .ForMember(x => x.AuthorsBooks, opts => opts.MapFrom(MapAuthorsBooks));
        CreateMap<Book, GetBookDto>();
        CreateMap<Book, GetBookDtoWithAuthors>()
            .ForMember(x => x.Authors, opts => opts.MapFrom(MapGetBookDtoAuthors));
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, GetCommentDto>();
        CreateMap<PatchBookDto, Book>().ReverseMap();
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

    private List<GetAuthorDto> MapGetBookDtoAuthors(Book book, GetBookDto getBookDto)
    {
        var result = new List<GetAuthorDto>();
        if (book.AuthorsBooks is null) return result;

        foreach(var authorBook in book.AuthorsBooks)
        {
            result.Add(new GetAuthorDto()
            {
                Id = authorBook.AuthorId,
                Name = authorBook.Author.Name
            });
        }

        return result;
    }

    private List<GetBookDto> MapGetAuthorDtoBooks(Author author, GetAuthorDto getAuthorDto)
    {
        var result = new List<GetBookDto>();
        if (author.AuthorsBooks is null) return result;

        foreach(var authorBook in author.AuthorsBooks)
        {
            result.Add(new GetBookDto()
            {
                Id = authorBook.AuthorId,
                Title = authorBook.Book.Title
            });
        }

        return result;
    }
}
