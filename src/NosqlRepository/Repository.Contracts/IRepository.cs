namespace Platform.Infrastructure.NoSql.Repository.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Platform.Infrastructure.NoSql.Repository.Contracts.Models;

    /// <summary>Repository interface which will allow user to invoke different kind of repository.</summary>
    public interface IRepository
    {
        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Qurable expression.</returns>
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Gets the items asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Qurable task.</returns>
        Task<IQueryable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Gets the items.</summary>
        /// <typeparam name="T">Poco object.</typeparam>
        /// <returns>Qurable expression.</returns>
        IQueryable<T> GetItems<T>();

        /// <summary>Gets the items asynchronous.</summary>
        /// <typeparam name="T">Poco object.</typeparam>
        /// <returns>Qurable task.</returns>
        Task<IQueryable<T>> GetItemsAsync<T>();

        /// <summary>Saves the specified data.</summary>
        /// <typeparam name="T">Poco object.</typeparam>
        /// <param name="data">The data.</param>
        void Save<T>(T data);

        /// <summary>Saves the specified data.</summary>
        /// <typeparam name="T">List of poco object.</typeparam>
        /// <param name="data">The data.</param>
        void Save<T>(List<T> data);

        /// <summary>Saves the asynchronous.</summary>
        /// <typeparam name="T">Poco object.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Task.</returns>
        Task SaveAsync<T>(T data);

        /// <summary>Saves the asynchronous.</summary>
        /// <typeparam name="T">List of poco object.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>Task.</returns>
        Task SaveAsync<T>(List<T> data);

        /// <summary>Gets the item.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Poco type.</returns>
        T GetItem<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Gets the item asynchronous.</summary>
        /// <typeparam name="T">Expressiin.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Task.</returns>
        Task<T> GetItemAsync<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Deletes the specified data filters.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        void Delete<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Deletes the asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync<T>(Expression<Func<T, bool>> dataFilters);

        /// <summary>Updates the specified data filters.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        bool Update<T>(Expression<Func<T, bool>> dataFilters, T data);

        /// <summary>Updates doucument partially.</summary>
        /// <typeparam name="T">
        ///   <br />
        /// </typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        bool UpdatePartial<T>(Expression<Func<T, bool>> dataFilters, T data);

        /// <summary>Updates the asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task.</returns>
        Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> dataFilters, T data);

        /// <summary>Updates the many asynchronous.</summary>
        /// <typeparam name="T">Expression.</typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task UpdateManyAsync<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "");

        /// <summary>Updates doucument partially.</summary>
        /// <typeparam name="T">
        ///   <br />
        /// </typeparam>
        /// <param name="dataFilters">The data filters.</param>
        /// <param name="data">The data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> UpdatePartialAsync<T>(Expression<Func<T, bool>> dataFilters, object data);

        /// <summary>Executes the command.</summary>
        /// <param name="command">The command.</param>
        /// <returns>ExecutedCommandResponse.</returns>
        ExecutedCommandResponse ExecuteCommand(string command);

        /// <summary>Executes the command asynchronous.</summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task<ExecutedCommandResponse> ExecuteCommandAsync(string command);
    }
}
