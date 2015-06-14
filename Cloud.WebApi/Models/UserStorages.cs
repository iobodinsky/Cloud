using System.Collections.Generic;
using Cloud.Repositories.DataContext;

namespace Cloud.WebApi.Models {
	public class UserStorages {
		public IEnumerable<Storage> Connected { get; set; }
		public IEnumerable<Storage> Available { get; set; }
	}
}