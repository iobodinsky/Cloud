using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
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
                new Logger().LogException(ex);
            }
            catch (Exception ex)
            {
                new Logger().LogException(ex);
            }
        }

        /// <summary>
        ///    Generic acync method to add entity to the data context
        /// </summary>
        /// <param name="entity">The entity that has to be added to the data context</param>
        /// <param name="isAutoSave">If AutoSave is true the entity will utomatically be saved to the database</param>
        /// <typeparam name="T">Entity type</typeparam>
        public async Task AddAsync<T>(T entity, bool isAutoSave) where T : class
        {
            await Task.Run(() =>
            {
                var dbSet = Entities.Set<T>();
                if (dbSet == null) throw new NullReferenceException("Entities.Set<T>");

                dbSet.Add(entity);
                if (isAutoSave) SaveChanges();
            });
        }

        /// <summary>
        ///    Generic method to add entity to the data context
        /// </summary>
        /// <param name="entity">The entity that has to be added to the data context</param>
        /// <param name="isAutoSave">If AutoSave is true the entity will utomatically be saved to the database</param>
        /// <typeparam name="T">Entity type</typeparam>
        public void Add<T>(T entity, bool isAutoSave) where T : class
        {
            var dbSet = Entities.Set<T>();
            if (dbSet == null) throw new NullReferenceException("Entities.Set<T>");

            dbSet.Add(entity);
            if (isAutoSave) SaveChanges();
        }

        public IStorage ResolveStorageInstance(int storageId)
        {
            var storageEntity = Entities.Storages.
                SingleOrDefault(server => server.Id == storageId);
            if (storageEntity == null) throw new NullReferenceException("storageEntity");
            var storageType = Type.GetType(storageEntity.ClassName, true);
            if (storageType == null) throw new NullReferenceException("storageType");
            var storageInstance = Activator.CreateInstance(storageType, storageId) as IStorage;

            return storageInstance;
        }

        public IStorage ResolveStorageInstance(int storageId, string className)
        {
            var storageType = Type.GetType(className, true);
            if (storageType == null) throw new NullReferenceException("storageType");
            var storage = Activator.CreateInstance(storageType, storageId) as IStorage;

            return storage;
        }
    }
}