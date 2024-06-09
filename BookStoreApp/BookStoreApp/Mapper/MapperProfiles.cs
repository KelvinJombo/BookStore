using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Book;
using BookStore.Domain.Entities;

namespace BookStoreApp.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            CreateMap<AddBookDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());             
            CreateMap<Book, BookResponseDto>();
            CreateMap<UpdateBookDto, Book>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));             
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<Order, OrderResponseDto>();
        }
    }
}
