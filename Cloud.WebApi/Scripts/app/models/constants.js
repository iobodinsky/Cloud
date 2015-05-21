window.cloud = window.cloud || {};
cloud.models = cloud.models || {};

cloud.models.constants = cloud.models.constants || {
	userTokenKey: 'CloudUserBearerToken',
	userTokenType: 'Bearer',
	urls: {
		common: {
			constructUpload: function (cloudId, folderId) {
				return 'api/files/cloud/' + cloudId + '/folder/' + folderId + '/upload';
			},
			constructRename: function(fileId, cloudId ) {
				return 'api/files/' + fileId + '/cloud/' + cloudId + '/rename';
			}
		},
		cloud: {
			home: '',
			token: '/Token',
			register: 'api/Account/Register',
			userInfo: 'api/Account/UserInfo',
			files: 'api/files',
		},
		drive: {
			driveUpload: 'https://www.googleapis.com/upload/drive/v2/files?uploadType=multipart'
		}
	}
};