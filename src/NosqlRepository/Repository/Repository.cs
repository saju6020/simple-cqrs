// <copyright file="Repository.cs" company="Shohoz">
// Copyright © 2015-2020 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Shohoz.Platform.Infrastructure.Repository.Contracts;
    using Shohoz.Platform.Infrastructure.Repository.Contracts.Models;

    /// <summary>Sample repository for other kind of db.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.Repository.Contracts.IRepository" />
    public class Repository : IRepository
    {
        public void Delete<T>(Expression<Func<T, bool>> dataFilters)
        {
            throw new NotImplementedException();
        }

        public ExecutedCommandResponse ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }

        public T GetItem<T>(Expression<Func<T, bool>> dataFilters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetItems<T>()
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T data)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(List<T> data)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, List<T> data)
        {
            throw new NotImplementedException();
        }
    }
}
