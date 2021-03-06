﻿using System;
using System.Data;
using System.Data.Entity;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Базовый класс для репозиториев
    /// </summary>
    public abstract class RepositoryBase<T> : IDisposable where T : class
    {
        /// <summary>
        /// Контекст доступа к данным
        /// </summary>
        private IpGeoBaseContext _dataContext = null;

        public IpGeoBaseContext DataContext
        {
            get { return _dataContext; }
            internal set { _dataContext = value; }
        }

        public RepositoryBase(IpGeoBaseContext dataContext)
        {
            _dataContext = dataContext;
        }

        public RepositoryBase()
            : this(new IpGeoBaseContext())
        {
        }

        public abstract DbSet<T> GetDbSet();

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <param name="entity">Сохраняемая сущность</param>
        public virtual void Create(T entity)
        {
            GetDbSet().Add(entity);
            DataContext.SaveChanges();
        }

        /// <summary>
        /// Сохраняет сущность в базе данных
        /// </summary>
        /// <param name="entity">Сохраняемая сущность</param>
        public virtual void Save(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        /// <summary>
        /// Удаляет сущность из базы данных
        /// </summary>
        /// <param name="newsitem">Удаляемая сущность</param>
        public void Delete(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Deleted;
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && _dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
            }
        }
    }
}
