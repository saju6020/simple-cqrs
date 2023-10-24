using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPostService
    {
        public void SavePost(PostDto postDto);
        
    }
}
