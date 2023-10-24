using AutoMapper;
using Blog.ORM.Models;
using MediatR;
using Platform.Infrastructure.Repository.Contract;
using SimpleCQRS.Blog.Domain.Commands;

namespace SimpleCQRS.Blog.Domain.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {

        public CreatePostCommandHandler(IRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public async Task<Guid> Handle(CreatePostCommand createPostCommand, CancellationToken cancellationToken)
        {
            var blog = this._mapper.Map<BlogDetails>(createPostCommand);
            blog.CreatedOn = DateTime.UtcNow;

            await this._repository.CreateAsync<BlogDetails>(blog);
            await this._repository.SaveAsync();

            return blog.BlogId;
        }

    }
}
