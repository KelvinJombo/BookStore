using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Domain.Entities;

namespace BookStoreApp.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            CreateMap<AddBookDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            //CreateMap<AddBookDto, Book>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<UpdateBookDto, Book>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            //CreateMap<UpdateBookDto, Book>().ReverseMap();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
        }
    }
}
