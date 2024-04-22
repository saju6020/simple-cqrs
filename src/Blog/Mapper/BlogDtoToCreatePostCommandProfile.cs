using AutoMapper;
using Blog.ORM.Models;
using SimpleCQRS.Blog.Domain.Commands;
using SimpleCQRS.Blog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Mapper
{
    public class BlogDtoToCreatePostCommandProfile : Profile
    {
        public BlogDtoToCreatePostCommandProfile()
        {
            CreateMap<BlogDto, CreatePostCommand>();
        }
    }
}
