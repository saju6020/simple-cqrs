using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.UAM.Database;
using SimpleCQRS.UAM.Domain.CommandHandlers;
using SimpleCQRS.UAM.Domain.Commands;

namespace NUnitCoreTest
{
    public class CreateUserCommandHandlerTest
    {
        private  Mock<IMapper> _mapper;
        private  Mock<UserManager<User>> _userManager;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<User>(It.IsAny<CreateUserCommand>()))
                .Returns<CreateUserCommand>(commmand =>
                {
                    if (commmand == null)
                    {
                        return null;
                    }

                    return new User
                    {
                        UserName = commmand.UserName,
                        Email = commmand.Email
                    };
                });
        }


        [Test]
        public async Task User_Create_ThrowException()
        {
            CreateUserCommand createUserCommand = null;


            CreateUserCommandHandler createUserCommandHandler = new CreateUserCommandHandler(_userManager.Object, _mapper.Object);


            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await createUserCommandHandler.HandleAsync(createUserCommand);
            });

                    
        }


        [Test]
        public async Task User_Create_Successfully()
        {
            CreateUserCommand createUserCommand = new CreateUserCommand
            {
                UserName = "Md Shahjahan",
                Email = "saju_6020@gmail.com"
            };


            CreateUserCommandHandler createUserCommandHandler = new CreateUserCommandHandler(_userManager.Object, _mapper.Object);
            var commandResponse = await createUserCommandHandler.HandleAsync(createUserCommand);

            Assert.NotNull(commandResponse.Result);
        }
    }
}