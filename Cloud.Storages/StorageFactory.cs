using System;
using System.Linq;
using Cloud.Common.Interfaces;
using Cloud.Repositories.Repositories;

namespace Cloud.Storages
{
    public class StorageFactory
    {
        private readonly UserStorageRepository _userStorageRepository;

        public StorageFactory()
        {
            _userStorageRepository = new UserStorageRepository();
        }

        public IStorage ResolveInstance(int storageId)
        {
            var storageEntity = _userStorageRepository.Entities.Storages.
                SingleOrDefault(server => server.Id == storageId);
            if (storageEntity == null) throw new NullReferenceException("storageEntity");
            var storageType = Type.GetType(storageEntity.ClassName, true);
            if (storageType == null) throw new NullReferenceException("storageType");
            var storageInstance = Activator.CreateInstance(storageType, storageId) as IStorage;

            return storageInstance;
        }

        public IStorage ResolveInstance(int storageId, string className)
        {
            var storageType = Type.GetType(className, true);
            if (storageType == null) throw new NullReferenceException("storageType");
            var storage = Activator.CreateInstance(storageType, storageId) as IStorage;

            return storage;
        }
    }
}
