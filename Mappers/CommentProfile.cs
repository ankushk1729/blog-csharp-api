using AutoMapper;
using SM.Dtos;
using SM.Entities;

namespace SM.Mappers
{
    public class CommentProfile : Profile
    {
        public CommentProfile(){
            CreateMap<CreateCommentDto, Comment>();
        }
    }
}