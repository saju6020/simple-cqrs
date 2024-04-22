using AutoMapper;
using Blog.ORM.Models;
using Platform.Infrastructure.Core;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.Blog.Domain.Commands;
using Platform.Infrastructure.Repository.EF;

namespace SimpleCQRS.Blog.Domain.CommandHandlers
{
    public class CreatePostCommandHandler : ICommandHandlerAsync<CreatePostCommand>
    {

        public CreatePostCommandHandler(IOrmRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        private readonly IOrmRepository _repository;
        private readonly IMapper _mapper;      

       public async Task<CommandResponse> HandleAsync(CreatePostCommand createPostCommand)
        {
            var blog = this._mapper.Map<BlogDetails>(createPostCommand);
            blog.CreatedOn = DateTime.UtcNow;

            await this._repository.CreateAsync<BlogDetails>(blog);
            await this._repository.CommitAsync();

            return new CommandResponse()
            {
                Result = blog.Id
            };
        }

    }
}
