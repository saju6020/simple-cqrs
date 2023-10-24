using Dtos;
using Platform.Infrastructure.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PostService : IPostService
    {
        private readonly IRepository _repository;

        public PostService(IRepository repository)
        {
            this._repository = repository;
        }

        public void SavePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }
    }
}
