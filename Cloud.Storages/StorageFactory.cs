using System;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Common.Models;
using Cloud.Repositories.Repositories;
using Cloud.Storages.Dropbox;
using Cloud.Storages.GoogleDrive;

namespace Cloud.Storages
{
    public class StorageFactory
    {
        private readonly UserStorageRepository _userStorageRepository;

        public StorageFactory()
        {
            _userStorageRepository = new UserStorageRepository();
        }

        public IStorage ResolveInstance(string alias)
        {
            var storageEntity = _userStorageRepository.Entities.Storages.
                SingleOrDefault(storage => storage.Alias == alias);
            if (storageEntity == null) throw new NullReferenceException("storageEntity");
            var storageType = Type.GetType(storageEntity.ClassName, true);
            if (storageType == null) throw new NullReferenceException("storageType");
            var storageInstance = Activator.CreateInstance(storageType, storageEntity.Id, alias) as IStorage;

            return storageInstance;
        }

        public GoogleDriveStorage GetGoogleDriveInstance()
        {
            return new GoogleDriveStorage(Constants.GoogleDriveStorageId, Constants.GoogleDriveStorageAlias);
        }

        public DropboxStorage GetDropboxInstance()
        {
            return new DropboxStorage(Constants.DropboxStorageId, Constants.DropboxStorageAlias);
        }
    }
}