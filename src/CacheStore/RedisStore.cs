namespace Platform.Infrastructure.CacheStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using ProtoBuf;
    using Platform.Infrastructure.CacheStore.Contracts;
    using Platform.Infrastructure.CacheStore.Serializers;
    using Platform.Infrastructure.CustomException;
    using StackExchange.Redis;

    /// <summary>This class will provide functionality to store data in redis.</summary>
    public class RedisStore : ICacheStore
    {
        private readonly IDatabase redisDataBase;
        private readonly ILogger<RedisStore> logger;

        /// <summary>Initializes a new instance of the <see cref="RedisStore"/> class.</summary>
        /// <param name="database">The database.</param>
        /// <param name="logger">The logger instance for this class.</param>
        public RedisStore(IDatabase database, ILogger<RedisStore> logger)
        {
            this.redisDataBase = database;
            this.logger = logger;
        }

        /// <summary>Hashes the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="hashKey">The hashkey.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns>bool success.</returns>
        public async Task<Task<bool>> HashSetAsync<T>(string key, string hashKey, T provider, DateTime? expiry) // dictionary<key,dictionary<hash,value>>...
        {
            return await this.redisDataBase.HashSetAsync(key, hashKey, ProtobufSerializer.Serialize(provider))
                .ContinueWith(async t => await this.redisDataBase.KeyExpireAsync(key, expiry));
        }

        /// <summary>Hashes the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>bool success.</returns>
        public async Task<bool> HashSetAsync<T>(string key, string hashKey, T provider) // dictionary<key,dictionary<hash,value>>...
        {
            return await this.redisDataBase.HashSetAsync(key, hashKey, ProtobufSerializer.Serialize(provider));
        }

        /// <summary>Sets the add asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="expiry">The expiry.</param>
        /// <returns>bool success.</returns>
        public async Task<Task<bool>> SetAddAsync<T>(string key, T provider, DateTime? expiry) // <key,List<value>> appends new value on same key....for intersection and etc functionalities
        {
            return await this.redisDataBase.SetAddAsync(key, ProtobufSerializer.Serialize(provider))
                .ContinueWith(async t => await this.redisDataBase.KeyExpireAsync(key, expiry));
        }

        /// <summary>Sets the add asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>bool success.</returns>
        public async Task<bool> SetAddAsync<T>(string key, T provider)
        {
            // <key,List<value>> appends new value on same key....for intersection and etc functionalities
            return await this.redisDataBase.SetAddAsync(key, ProtobufSerializer.Serialize(provider));
        }

        /// <summary>Sets the add asynchronous.</summary>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>bool success.</returns>
        public async Task<bool> SetAddAsync(string key, string provider)
        {
            // <key,List<value>> appends new value on same key....for intersection and etc functionalities
            return await this.redisDataBase.SetAddAsync(key, ProtobufSerializer.Serialize(provider));
        }

        /// <summary>Strings the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="expiry">The expiry.</param>
        /// <param name="when">The when.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>bool success.</returns>
        public async Task<bool> StringSetAsync<T>(string key, T provider, TimeSpan? expiry, When when = When.Always, CommandFlags flags = CommandFlags.None) // normal <key,value>
        {
            return await this.redisDataBase.StringSetAsync(key, ProtobufSerializer.Serialize(provider), expiry, when: when, flags: flags);
        }

        /// <summary>Strings the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="when">The when.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>bool success.</returns>
        public async Task<bool> StringSetAsync<T>(string key, T provider, When when = When.Always, CommandFlags flags = CommandFlags.None) // normal <key,value>
        {
            return await this.redisDataBase.StringSetAsync(key, ProtobufSerializer.Serialize(provider), when: when, flags: flags);
        }

        /// <summary>Finds the asynchronous.</summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IEnumerable<T>> FindAsync<T>(RedisKey[] keys)
        {
            ITransaction transaction = this.redisDataBase.CreateTransaction();

            List<T> providers = new List<T>();

            List<Task<HashEntry[]>> tasks = new List<Task<HashEntry[]>>();

            foreach (RedisKey key in keys)
            {
                tasks.Add(transaction.HashGetAllAsync(key));
            }

            bool success = await transaction.ExecuteAsync();

            if (success)
            {
                foreach (Task<HashEntry[]> task in tasks)
                {
                    List<T> fetchedProvider = (await task).
                        Select(p =>
                        Serializer.Deserialize<T>(p.Value))
                        .ToList();
                    providers.AddRange(fetchedProvider);
                }
            }

            return await Task.FromResult<IEnumerable<T>>(providers);
        }

        /// <summary>
        /// HGetAllAsync.
        /// </summary>
        /// <param name="key"> Redis Key.</param>
        /// <returns>Task of HashsetEntry.</returns>
        public async Task<HashEntry[]> HGetAllAsync(RedisKey key)
        {
            return await this.redisDataBase.HashGetAllAsync(key);
        }

        /// <summary>
        /// HSetAllAsync.
        /// </summary>
        /// <param name="key"> Redis Key.</param>
        /// <param name="hashEntries"> Redis HashEntries.</param>
        /// <param name="commandFlags"> Redis CommandFlags.</param>
        /// <returns>Task.</returns>
        public async Task HSetAllAsync(RedisKey key, HashEntry[] hashEntries, CommandFlags commandFlags = CommandFlags.None)
        {
            await this.redisDataBase.HashSetAsync(key, hashEntries, commandFlags).ConfigureAwait(false);
        }

        /// <summary>
        /// HGet.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="hashField">Field.</param>
        /// <param name="commandFlags">CommandFlags.</param>
        /// <returns>RedisValue.</returns>
        public RedisValue HGet(RedisKey key, RedisValue hashField, CommandFlags commandFlags = CommandFlags.None)
        {
            return this.redisDataBase.HashGet(key, hashField, commandFlags);
        }

        public async Task<bool> HDelete(RedisKey redisKey, RedisValue redisValue)
        {
            return await this.redisDataBase.HashDeleteAsync(redisKey, redisValue).ConfigureAwait(false);
        }

        /// <summary>Finds the asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>Return generic list.</returns>
        public async Task<IEnumerable<T>> FindAsync<T>(RedisKey keys)
        {
            return (await this.redisDataBase.HashGetAllAsync(keys))
                .Select(p =>
                    ProtobufSerializer.Deserialize<T>(p.Value));
        }

        /// <summary>Finds the asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">the key.</param>
        /// <returns>Return generic list.</returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            RedisValue result = await this.redisDataBase.StringGetAsync(key);

            return ProtobufSerializer.Deserialize<T>(result);
        }

        /// <summary>Find RedisKeys matching the pattern..</summary>
        /// <param name="pattern">the pattern.</param>
        /// <returns>Return list of RedisKey IAsyncEnumerable.</returns>
        public List<IAsyncEnumerable<RedisKey>> GetKeysMatchingPattern(string pattern)
        {
            IConnectionMultiplexer muxer = this.redisDataBase.Multiplexer;
            EndPoint[] endPoints = this.redisDataBase.Multiplexer.GetEndPoints();
            List<IAsyncEnumerable<RedisKey>> keys = new List<IAsyncEnumerable<RedisKey>>();
            foreach (EndPoint endPoint in endPoints)
            {
                IServer server = muxer.GetServer(endPoint);
                keys.Add(server.KeysAsync(database: this.redisDataBase.Database, pattern: pattern));
            }

            return keys;
        }

        /// <summary>Strings the get json asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Returns generic object deserialized from json.</returns>
        public async Task<T> StringGetJsonAsync<T>(RedisKey key)
        {
            try
            {
                RedisValue result = await this.redisDataBase.StringGetAsync(key);
                if (result.IsNullOrEmpty)
                {
                    this.logger.LogError($"Data does not exist for this key: {key}");
                    throw new BaseException(
                        $"Data does not exist for this key: {key}",
                        new NullReferenceException(nameof(T)));
                }

                this.logger.LogInformation($"Deserializing now.");
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (JsonException e)
            {
                this.logger.LogError("Deserialization failed");
                this.logger.LogError(e.Message);
                throw new BaseException(e.Message, e);
            }
        }

        /// <summary>Finds the asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="keys">the keys.</param>
        /// <returns>Return list of generic object deserialized from json.</returns>
        public async Task<List<T>> StringGetJsonAsync<T>(List<RedisKey> keys)
        {
            try
            {
                RedisValue[] result = await this.redisDataBase.StringGetAsync(keys.ToArray());
                this.logger.LogInformation($"Deserializing now.");
                return result
                    .Where(x => x.HasValue)
                    .Select(x => JsonConvert.DeserializeObject<T>(x))
                    .ToList();
            }
            catch (JsonException e)
            {
                this.logger.LogError("Deserialization failed");
                this.logger.LogError(e.Message);
                throw new BaseException(e.Message, e);
            }
        }

        /// <summary>
        /// Deletes provided keys from redis.
        /// </summary>
        /// <param name="keys">The RedisKey array.</param>
        /// <param name="commandFlags">The CommandFlags.</param>
        /// <returns>Task of deleted keys numbers as long.</returns>
        public async Task<long> DeleteKey(RedisKey[] keys, CommandFlags commandFlags = CommandFlags.None)
        {
            return await this.redisDataBase.KeyDeleteAsync(keys, commandFlags).ConfigureAwait(false);
        }

        /// <summary>
        /// GeoAdds member under provided key.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="longitude">Latitude of the member.</param>
        /// <param name="latitude">Longitude of the member.</param>
        /// <param name="member">Member.</param>
        /// <param name="command">The commandflags..</param>
        /// <returns>Task of boolean.</returns>
        public async Task<bool> GeoAddAsync(
            RedisKey key,
            double longitude,
            double latitude,
            RedisValue member,
            CommandFlags command = CommandFlags.None)
        {
            return await this.redisDataBase.GeoAddAsync(key, longitude, latitude, member, command).ConfigureAwait(false);
        }

        /// <summary>
        /// GeoAdds member under provided key.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="geoEntries">Collection of GeoEntries.</param>
        /// <param name="command">The commandflags..</param>
        /// <returns>Task of long.</returns>
        public async Task<long> GeoAddAsync(
            RedisKey key,
            GeoEntry[] geoEntries,
            CommandFlags command = CommandFlags.None)
        {
            return await this.redisDataBase.GeoAddAsync(key, geoEntries, command).ConfigureAwait(false);
        }

        /// <summary>
        /// Does GeoRadius search against the provided RedisKey.
        /// </summary>
        /// <param name="key">The RedisKey.</param>
        /// <param name="latitude">The Latitude.</param>
        /// <param name="longitude">The Longitude.</param>
        /// <param name="radius">The Radius.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="count">The Count.</param>
        /// <param name="order">The Order.</param>
        /// <param name="options">The Options.</param>
        /// <param name="commandFlags">The CommandFlags.</param>
        /// <returns>Task of GeoRadiusResult collection.</returns>
        public async Task<GeoRadiusResult[]> GeoRadiusAsync(
            RedisKey key,
            double latitude,
            double longitude,
            double radius,
            GeoUnit unit = GeoUnit.Meters,
            int count = -1,
            Order order = Order.Ascending,
            GeoRadiusOptions options = GeoRadiusOptions.Default,
            CommandFlags commandFlags = CommandFlags.None)
        {
            return await this.redisDataBase.GeoRadiusAsync(
                 key,
                 longitude: longitude,
                 latitude: latitude,
                 radius,
                 unit,
                 count,
                 order,
                 options,
                 commandFlags).ConfigureAwait(false);
        }

        /// <summary>
        /// Does ZUnionStore.
        /// </summary>
        /// <param name="operation">operation type. Like union/intersect.</param>
        /// <param name="destinationKey">The destination key.</param>
        /// <param name="sourceKeys">The source keys.</param>
        /// <param name="weights">weights of each sorted set.</param>
        /// <param name="aggregate">The aggregate option. Like sum/min/max.</param>
        /// <param name="commandFlags">The CommandFlags.</param>
        /// <returns>Returns Task of number of elements in the destination sorted set.</returns>
        public async Task<long> ZUnionStoreAsync(
            SetOperation operation,
            RedisKey destinationKey,
            RedisKey[] sourceKeys,
            double[] weights,
            Aggregate aggregate,
            CommandFlags commandFlags = CommandFlags.None)
        {
            return await this.redisDataBase.SortedSetCombineAndStoreAsync(
                 operation,
                 destination: destinationKey,
                 keys: sourceKeys,
                 weights: weights,
                 aggregate,
                 commandFlags).ConfigureAwait(false);
        }

        /// <summary>
        /// Does ZRangeByScore operation on provided key.
        /// </summary>
        /// <param name="key">The RedisKey.</param>
        /// <param name="start">The start position.</param>
        /// <param name="stop">The end position.</param>
        /// <param name="exclude">The Exclusion options.</param>
        /// <param name="order">The order option.</param>
        /// <param name="skip">Number of items to be skipped.</param>
        /// <param name="take">Number of items to take. -1 means all.</param>
        /// <param name="commandFlags">The commandflag. </param>
        /// <returns>Returns Task of SortedSetEntries.</returns>
        public async Task<SortedSetEntry[]> ZRangeByScoreAsync(
            RedisKey key,
            double start = double.NegativeInfinity,
            double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None,
            Order order = Order.Ascending,
            long skip = 0,
            long take = -1,
            CommandFlags commandFlags = CommandFlags.None)
        {
            return await this.redisDataBase.SortedSetRangeByScoreWithScoresAsync(
                key,
                start,
                stop,
                exclude,
                order,
                skip,
                take,
                commandFlags).ConfigureAwait(false);
        }

        public IDatabase GetRedisDataBase()
        {
            return this.redisDataBase;
        }
    }
}
