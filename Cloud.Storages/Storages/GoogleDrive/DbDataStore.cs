using System.Threading.Tasks;
using Cloud.Storages.DataContext;
using Cloud.Storages.Repositories;
using Google.Apis.Json;
using Google.Apis.Util.Store;

namespace Cloud.Storages.Storages.GoogleDrive {
	internal class DbDataStore : IDataStore {

		private readonly GoogleDriveTokenRepository _repository;

		public DbDataStore() {
			_repository = new GoogleDriveTokenRepository();
		}

		public async Task StoreAsync<T>( string key, T value ) {
			var contents = NewtonsoftJsonSerializer.Instance.Serialize(value);
			var token = new GoogleDriveUserToken {
				UserId = key,
				Tokens = contents
			};

			await _repository.AddAsync(token, true);
		}

		public async Task DeleteAsync<T>( string key ) {
			await _repository.DeleteAsync(key);
		}

		public async Task<T> GetAsync<T>( string key ) {
			var completionSource = new TaskCompletionSource<T>();
			var token = await _repository.GetTokenAsync(key);
			if (token == null) {
				completionSource.SetResult(default(T));
			} else {
				completionSource.SetResult(NewtonsoftJsonSerializer.Instance
					.Deserialize<T>(token.Tokens));
			}

			return completionSource.Task.Result;
		}

		public Task ClearAsync() {
			throw new System.NotImplementedException();
		}
	}
}