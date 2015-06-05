window.cloud = window.cloud || {};
cloud.models = cloud.models || {};

cloud.models.constants = cloud.models.constants || {
	userTokenKey: 'CloudUserBearerToken',
	userTokenType: 'Bearer',
	cloudId: 2,
	urls: {
		cloud: {
			home: '',
			token: '/Token',
			register: 'api/Account/Register',
			userInfo: 'api/Account/UserInfo',
			files: {
				constructUpload: function (folderId, cloudId) {
					return 'api/files/cloud/' + cloudId + '/folder/' + folderId + '/upload';
				},
				constructRename: function (fileId, cloudId) {
					return 'api/files/' + fileId + '/cloud/' + cloudId + '/rename';
				},
				constructDelete: function (fileId, cloudId) {
					return 'api/files/' + fileId + '/cloud/' + cloudId + '/delete';
				},
			},
			folders: {
				getRoot: 'api/folders/root',
				constructCreate: function(cloudId) {
					return 'api/folders/cloud/' + cloudId + '/create';
				},
				constructFolderData: function (folderId, cloudId) {
					return 'api/folders/' + folderId + '/cloud/' + cloudId;
				},
				constructDelete: function (folderId, cloudId) {
					return 'api/folders/' + folderId + '/cloud/' + cloudId + '/delete';
				}
			},
		},
		drive: {
			driveUpload: 'https://www.googleapis.com/upload/drive/v2/files?uploadType=multipart'
		}
	}
};