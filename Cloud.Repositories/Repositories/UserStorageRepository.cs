﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloud.Repositories.DataContext;

namespace Cloud.Repositories.Repositories
{
    public class UserStorageRepository : RepositoryBase
    {
        public IEnumerable<Storage> GetStorages(string userId)
        {
           return GetConnectedUserStorages(userId);
        }

        public IEnumerable<Storage> GetConnectedUserStorages(string userId)
        {
            return Entities.AspNetUsers_Storages.Where(
                userStorage => userStorage.Storage.IsActive &&
                               userStorage.UserId == userId)
                .Select(userStorage => userStorage.Storage);
        }

        public IEnumerable<Storage> GetAvailableUserStorages(string userId)
        {
            return Entities.Storages.Where(storage => storage.IsActive &&
                                                      storage.AspNetUsers_Storages
                                                          .All(userStorage => userStorage.UserId != userId));
        }

        public async Task AddAsync(string userId, int storageId)
        {
            await Task.Run(() =>
            {
                Entities.AspNetUsers_Storages.Add(new AspNetUsers_Storages
                {
                    UserId = userId,
                    StorageId = storageId
                });

                Entities.SaveChanges();
            });
        }

        public async Task DeleteAsync(string userId, int storageId)
        {
            await Task.Run(() =>
            {
                var entity = Entities.AspNetUsers_Storages.SingleOrDefault(
                    userStorage => userStorage.UserId == userId && userStorage.StorageId == storageId);
                if (entity == null) return;
                Entities.AspNetUsers_Storages.Attach(entity);
                Entities.AspNetUsers_Storages.Remove(entity);
                Entities.SaveChanges();
            });
        }
    }
}