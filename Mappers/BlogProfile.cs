using AutoMapper;
using SM.Dtos;
using SM.Entities;

namespace SM.Mappers
{
    public class BlogProfile : Profile
    {
        public BlogProfile(){
            CreateMap<CreateBlogDto, Blog>();
        }
    }
}