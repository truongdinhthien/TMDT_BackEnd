using AutoMapper;
using BookApi.Core.Entity;
namespace BookApi.Core.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile () 
        {
            CreateMap<Book,BookDTO>();
            CreateMap<Category,CategoryDTO>();
        }
    }
}