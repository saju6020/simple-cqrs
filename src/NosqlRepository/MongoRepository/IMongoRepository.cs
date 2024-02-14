namespace Platform.Infrastructure.NoSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Platform.Infrastructure.NoSql.Repository.Contracts;

    /// <summary>Mongo repository abstraction.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.Repository.Contracts.IRepository" />
    public interface IMongoRepository : IRepository
    {
        /// <summary>
        /// Return MongoDB Collection Object.
        /// </summary>
        /// <typeparam name="T">Db model.</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Mongo Collection.</returns>
        IMongoCollection<T> GetCollection<T>(string collectionName);

        /// <summary>
        /// It reurns a List of MongDB Entity that matches filtering.
        /// It supports GeoSpacial query too.
        /// </summary>
        /// <typeparam name="T">MongoDB Entity.</typeparam>
        /// <param name="filterDefinition">Mongo Filter Definition.</param>
        /// <param name="collectionName">Exact name of the collection. Usually Collection name slightly differ from the EntityName.</param>
        /// <returns>A List of expected Entity that matched provided filter.</returns>
        Task<IEnumerable<T>> GetItems<T>(FilterDefinition<T> filterDefinition, string collectionName);

        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">Db model.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>List.</returns>
        Task<IEnumerable<T>> GetItems<T>(BsonDocument[] pipeline, string collectionName);

        /// <summary>
        /// Inserts data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="data">Data object.</param>
        /// <param name="collectionName">Name of the collection.</param>
        void Save<T>(T data, string collectionName);

        /// <summary>
        /// Inserts data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="data">List of data objects.</param>
        /// <param name="collectionName">Name of the collection.</param>
        void Save<T>(List<T> data, string collectionName);

        /// <summary>
        /// Async Insert.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="data">Data object.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Task.</returns>
        Task SaveAsync<T>(T data, string collectionName);

        /// <summary>Saves the asynchronous.</summary>
        /// <typeparam name="T">Data.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="data">The data.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task SaveAsync<T>(IClientSessionHandle session, T data, string collectionName = null);

        /// <summary>
        /// Async Insert.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="data">List of Data objects.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Task.</returns>
        Task SaveAsync<T>(List<T> data, string collectionName);

        /// <summary>
        /// Update data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="dataFilters">Lambda Expression.</param>
        /// <param name="data">Updated data Object.</param>
        /// <param name="collectionName">Name of the collection.</param>
        void Update<T>(Expression<Func<T, bool>> dataFilters, T data, string collectionName);

        /// <summary>
        /// Async Update data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="dataFilters">Lambda Expression.</param>
        /// <param name="data">Updated data Object.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data, string collectionName);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <typeparam name="T">Model.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync<T>(IClientSessionHandle session, Expression<Func<T, bool>> dataFilters, T data, string collectionName = null);

        /// <summary>
        /// Delete data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="dataFilters">Lambda expression.</param>
        /// <param name="collectionName">Name of the collection.</param>
        void Delete<T>(Expression<Func<T, bool>> dataFilters, string collectionName);

        /// <summary>
        /// Delete data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="dataFilters">Lambda expression.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters, string collectionName);

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <returns>IClientSessionHandle.</returns>
        IClientSessionHandle StartSession();
    }
}
