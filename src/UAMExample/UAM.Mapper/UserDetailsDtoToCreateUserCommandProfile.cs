using AutoMapper;
using SimpleCQRS.UAM.Domain.Commands;
using SimpleCQRS.UAM.Dtos;

namespace SimpleCQRS.UAM.Mapper
{
    public class UserDetailsDtoToCreateUserCommandProfile: Profile
    {
        public UserDetailsDtoToCreateUserCommandProfile()
        {
            CreateMap<UserDetailsDto, CreateUserCommand>();
        }
    }
}
