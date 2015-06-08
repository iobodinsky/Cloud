window.cloud = window.cloud || {};
cloud.models = cloud.models || {};

cloud.models.constants = cloud.models.constants || {
	userTokenKey: 'CloudUserBearerToken',
	userTokenType: 'Bearer',
	cloudId: 2,
	alert: {
		timeout: 3000,
		type: {
			success: 'success',
			info: 'info',
			warning: 'warning',
			danger: 'danger'
		},
		maxCount: 3
	},
	urls: {
		cloud: {
			home: '',
			token: '/Token',
			logout: 'api/Account/Logout',
			register: 'api/Account/Register',
			userInfo: 'api/Account/UserInfo',
			files: {
				constructUpload: function (folderId, cloudId) {
					return 'api/files/cloud/' + cloudId + '/folder/' + folderId + '/upload';
				},
				constructDownloadLink: function (fileId) {
					return 'api/files/' + fileId + '/requestlink';
				},
				constructRename: function(fileId, cloudId) {
					return 'api/files/' + fileId + '/cloud/' + cloudId + '/rename';
				},
				constructDelete: function(fileId, cloudId) {
					return 'api/files/' + fileId + '/cloud/' + cloudId + '/delete';
				},
			},
			folders: {
				rootFolderData: 'api/folders',
				constructCreate: function(cloudId) {
					return 'api/folders/cloud/' + cloudId + '/create';
				},
				constructRename: function(folderId, cloudId) {
					return 'api/folders/' + folderId + '/cloud/' + cloudId + '/rename';
				},
				constructFolderData: function(folderId, cloudId) {
					return 'api/folders/' + folderId + '/cloud/' + cloudId;
				},
				constructDelete: function(folderId, cloudId) {
					return 'api/folders/' + folderId + '/cloud/' + cloudId + '/delete';
				}
			},
		},
		drive: {
			driveUpload: 'https://www.googleapis.com/upload/drive/v2/files?uploadType=multipart'
		}
	},
	cloudEntities: {
		folder: 'folder',
		file: 'file'
	},
	message: {
		failLoadUserInfo: 'fail to load user info',
		failLogout: 'fail to logout',
		failDelete: 'fail to delete',
		failOpenFolder: 'fail to open folder',
		failUploadFile: 'fail to uploadFile',
		failGetRootFolderData: 'fail to get root folder',
		failCreatFolder: 'fail to create folder',
		failRename: 'fail to rename',
		failRegister: 'fail to register',
		failLogin: 'fail to login',
		failRequestDownloadLink: 'fail to download file',

		successfolderCreate: 'folder created',
		successUploadFile: 'file upload successed',
		successDelete: 'delete successed',
		successRename: 'rename successed',

		warningNotInCloudFolder: 'you cannot add new item in non cloud folder'
	}
};