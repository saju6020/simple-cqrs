using AutoMapper;
using Blog.ORM.Models;
using SimpleCQRS.Blog.Domain.Commands;

namespace SimpleCQRS.Blog.Mapper
{
    public class CreatePostCommandToDbModelProfile : Profile
    {
        public CreatePostCommandToDbModelProfile()
        {
            CreateMap<CreatePostCommand, BlogDetails>();
        }
    }
}
