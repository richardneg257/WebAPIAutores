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
        CreateMap<CreateBookDto, Book>();
        CreateMap<Book, GetBookDto>();
    }
}
