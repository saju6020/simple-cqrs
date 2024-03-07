namespace Platform.Infrastructure.CacheStore.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StackExchange.Redis;

    /// <summary>Interface for cache store.</summary>
    public interface ICacheStore
    {
        /// <summary>Strings the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="expiry">The expiry.</param>
        /// <param name="when">The when.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>bool success.</returns>
        Task<bool> StringSetAsync<T>(string key, T provider, TimeSpan? expiry, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>Strings the set asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="when">The when.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>bool success.</returns>
        Task<bool> StringSetAsync<T>(string key, T provider, When when = When.Always, CommandFlags flags = CommandFlags.None);

        /// <summary>Strings the get asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Generic type.</returns>
        Task<T> StringGetAsync<T>(string key);

        /// <summary>Find RedisKeys matching the pattern..</summary>
        /// <param name="pattern">the pattern.</param>
        /// <returns>Return list of RedisKey IAsyncEnumerable.</returns>
        List<IAsyncEnumerable<RedisKey>> GetKeysMatchingPattern(string pattern);

        /// <summary>Strings the get json asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Returns generic object deserialized from json.</returns>
        Task<T> StringGetJsonAsync<T>(RedisKey key);

        /// <summary>Finds the asynchronous.</summary>
        /// <typeparam name="T">generic type.</typeparam>
        /// <param name="keys">the keys.</param>
        /// <returns>Return list of generic object deserialized from json.</returns>
        Task<List<T>> StringGetJsonAsync<T>(List<RedisKey> keys);

        /// <summary>
        /// Deletes provided keys from redis.
        /// </summary>
        /// <param name="keys">The RedisKey array.</param>
        /// <param name="commandFlags">The CommandFlags.</param>
        /// <returns>Task of deleted keys numbers as long.</returns>
        public Task<long> DeleteKey(RedisKey[] keys, CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        /// GeoAdds member under provided key.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="longitude">Latitude of the member.</param>
        /// <param name="latitude">Longitude of the member.</param>
        /// <param name="member">Member.</param>
        /// <param name="command">The commandflags..</param>
        /// <returns>Task of boolean.</returns>
        public Task<bool> GeoAddAsync(
            RedisKey key,
            double longitude,
            double latitude,
            RedisValue member,
            CommandFlags command = CommandFlags.None);

        public Task<IEnumerable<T>> FindAsync<T>(RedisKey[] keys);

        /// <summary>
        /// GeoAdds member under provided key.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="geoEntries">Collection of GeoEntries.</param>
        /// <param name="command">The commandflags..</param>
        /// <returns>Task of long.</returns>
        public Task<long> GeoAddAsync(
            RedisKey key,
            GeoEntry[] geoEntries,
            CommandFlags command = CommandFlags.None);

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
        public Task<GeoRadiusResult[]> GeoRadiusAsync(
            RedisKey key,
            double latitude,
            double longitude,
            double radius,
            GeoUnit unit = GeoUnit.Meters,
            int count = -1,
            Order order = Order.Ascending,
            GeoRadiusOptions options = GeoRadiusOptions.Default,
            CommandFlags commandFlags = CommandFlags.None);

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
        public Task<long> ZUnionStoreAsync(
            SetOperation operation,
            RedisKey destinationKey,
            RedisKey[] sourceKeys,
            double[] weights,
            Aggregate aggregate,
            CommandFlags commandFlags = CommandFlags.None);

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
        public Task<SortedSetEntry[]> ZRangeByScoreAsync(
            RedisKey key,
            double start = double.NegativeInfinity,
            double stop = double.PositiveInfinity,
            Exclude exclude = Exclude.None,
            Order order = Order.Ascending,
            long skip = 0,
            long take = -1,
            CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        /// HGetAllAsync.
        /// </summary>
        /// <param name="key"> Redis Key.</param>
        /// <returns>Task of HashsetEntry.</returns>
        public Task<HashEntry[]> HGetAllAsync(RedisKey key);

        /// <summary>
        /// HGet.
        /// </summary>
        /// <param name="key">RedisKey.</param>
        /// <param name="hashField">Field.</param>
        /// <param name="commandFlags">CommandFlags.</param>
        /// <returns>RedisValue.</returns>
        public RedisValue HGet(RedisKey key, RedisValue hashField, CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        /// Deletes item from hash set.
        /// </summary>
        /// <param name="redisKey">The RedisKey.</param>
        /// <param name="redisValue">The RedisValue.</param>
        /// <returns>Returns Task.</returns>
        public Task<bool> HDelete(RedisKey redisKey, RedisValue redisValue);

        /// <summary>
        /// HSetAllAsync.
        /// </summary>
        /// <param name="key"> Redis Key.</param>
        /// <param name="hashEntries"> Redis HashEntries.</param>
        /// <param name="commandFlags"> Redis CommandFlags.</param>
        /// <returns>Task.</returns>
        public Task HSetAllAsync(RedisKey key, HashEntry[] hashEntries, CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        /// Get the Redis Database.
        /// </summary>
        /// <returns>IDatabase.</returns>
        public IDatabase GetRedisDataBase();
    }
}
