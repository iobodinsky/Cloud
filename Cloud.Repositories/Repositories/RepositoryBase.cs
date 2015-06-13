using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Common.Interfaces;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories {
	/// <summary>
	///    Base repository class
	/// </summary>
	public abstract class RepositoryBase {
		protected RepositoryBase() {
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
		public virtual void SaveChanges() {
			try {
				Entities.SaveChanges();
			} catch (DbEntityValidationException) {
			}

		}

		/// <summary>
		///    Generic method to add entity to the data context
		/// </summary>
		/// <param name="entity">The entity that has to be added to the data context</param>
		/// <param name="isAutoSave">If AutoSave is true the entity will utomatically be saved to the database</param>
		/// <typeparam name="T">Entity type</typeparam>
		public async Task AddAsync<T>( T entity, bool isAutoSave ) where T : class {
			await Task.Run(() => {
				var dbSet = Entities.Set<T>();
				if (dbSet == null) {
					// todo
					throw new Exception("todo");
				}
				dbSet.Add(entity);
				if (isAutoSave) {
					SaveChanges();
				}
			});
		}

		public IStorage ResolveStorageInstance( int cloudId ) {
			var cloudServer = Entities.Storages.
				SingleOrDefault(server => server.Id == cloudId);
			if (cloudServer == null) return null;
			var cloudType = Type.GetType(cloudServer.ClassName);
			if (cloudType == null) return null;
			var storage = Activator.CreateInstance(cloudType) as IStorage;

			return storage;
		}

		public IStorage ResolveStorageInstance( int cloudId, string className ) {
			var cloudType = Type.GetType(string.Concat(className, ", ", "Cloud.Storages"), true);
			if (cloudType == null) return null;
			var storage = Activator.CreateInstance(cloudType) as IStorage;

			return storage;
		}
	}
}