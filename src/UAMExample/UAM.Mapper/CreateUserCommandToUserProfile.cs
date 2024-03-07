using AutoMapper;
using SimpleCQRS.UAM.Database;
using SimpleCQRS.UAM.Domain.Commands;

namespace SimpleCQRS.UAM.Mapper
{
    public class CreateUserCommandToUserProfile : Profile
    {
        public CreateUserCommandToUserProfile()
        {
            CreateMap<CreateUserCommand, User>();
        }
    }
}
