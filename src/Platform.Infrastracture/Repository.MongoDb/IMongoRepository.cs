using MongoDB.Bson;
using Platform.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infrastructure.Repository.MongoDb
{
    public interface IMongoRepository : IRepository
    {
        public Task<IEnumerable<T>> GetItems<T>(BsonDocument[] pipeline, string collectionName);       
    }
}
