﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Web.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> SearchFor(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task Delete(T entity);
    }
}
