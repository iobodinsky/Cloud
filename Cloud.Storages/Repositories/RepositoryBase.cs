﻿using System.Data.Entity.Validation;
using Cloud.Storages.DataContext;

namespace Cloud.Storages.Repositories
{
     /// <summary>
    ///    Base repository class
    /// </summary>
    public abstract class RepositoryBase
    {
        protected RepositoryBase()
        {
            Entities = new CloudDbEntities();
            Entities.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        ///    Access Cloud entities
        /// </summary>
        public CloudDbEntities Entities { get; private set; }

        /// <summary>
        ///    Save context changes to the database
        /// </summary>
        public virtual void SaveChanges()
        {
            try
            {
                Entities.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
            }
            
        }

        /// <summary>
        ///    Generic method to add entity to the data context
        /// </summary>
        /// <param name="entity">The entity that has to be added to the data context</param>
        /// <param name="isAutoSave">If AutoSave is true the entity will utomatically be saved to the database</param>
        /// <typeparam name="T">Entity type</typeparam>
        public bool Add<T>(T entity, bool isAutoSave) where T : class
        {
            var dbSet = Entities.Set<T>();
            if (dbSet == null) return false;
            dbSet.Add(entity);
            if (isAutoSave)
            {
                SaveChanges();
            }

            return true;
        }
    }
}