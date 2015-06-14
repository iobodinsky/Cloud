window.cloud = window.cloud || {};
cloud.models = cloud.models || {};

cloud.models.constants = cloud.models.constants || {
	userTokenKey: 'CloudUserBearerToken',
	userTokenType: 'Bearer',
	storages: {
		cloudId: 2,
		googleDriveId: 1,
		dropboxId: 3
	},
	rootCloudFolderName: 'Cloud',
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
	httpMethod: {
		get: 'GET',
		post: 'POST',
		deleteMethod: 'DELETE',
	},
	httpHeader: {
		name: {
			authorization: 'Authorization',
			contentType: 'Content-Type'
		},
		value: {
			formUrlencoded: 'application/x-www-form-urlencoded;',
			json: 'application/json; charset=utf-8'
		}
	},
	urls: {
		cloud: {
			home: '',
			token: '/Token',
			logout: 'api/Account/Logout',
			register: 'api/Account/Register',
			userInfo: 'api/Account/UserInfo',
			storages: 'api/storages',
			authorize: 'api/storages/authorize/cloud',
			files: {
				constructUpload: function(folderId, storageId) {
					return 'api/files/cloud/' + storageId + '/folder/' + folderId + '/upload';
				},
				constructDownloadLink: function(fileId) {
					return 'api/files/' + fileId + '/requestlink';
				},
				constructRename: function(fileId, storageId) {
					return 'api/files/' + fileId + '/cloud/' + storageId + '/rename';
				},
				constructDelete: function(fileId, storageId) {
					return 'api/files/' + fileId + '/cloud/' + storageId + '/delete';
				},
			},
			folders: {
				rootFolderData: 'api/folders',
				constructCreate: function(storageId) {
					return 'api/folders/cloud/' + storageId + '/create';
				},
				constructRename: function(folderId, storageId) {
					return 'api/folders/' + folderId + '/cloud/' + storageId + '/rename';
				},
				constructFolderData: function(folderId, storageId) {
					return 'api/folders/' + folderId + '/cloud/' + storageId;
				},
				constructDelete: function(folderId, storageId) {
					return 'api/folders/' + folderId + '/cloud/' + storageId + '/delete';
				}
			},
		},
		drive: {
			authorize: 'api/storages/authorize/googledrive',
		},
		dropbox: {
			authorize: 'api/storages/authorize/dropbox',
		}
	},
	cloudEntities: {
		folder: 'folder',
		file: 'file'
	},
	message: {
		failLoadUserInfo: 'fail to load user info',
		failLogout: 'fail to logout',
		failOpenFolder: 'fail to open folder',
		failUploadFile: 'fail to uploadFile',
		failGetRootFolderData: 'fail to get root folder',
		failCreatFolder: 'fail to create folder',
		failRename: 'fail to rename',
		failRegister: 'fail to register',
		failLogin: 'fail to login',
		failRequestDownloadLink: 'fail to download file',
		failCloudNotFound: 'cloud not found',

		infoDriveFileDownloadingNotAllowed: 'download file not allowed',
		infoDropboxDownloadNotAllowed: 'download file from Dropbox not allowed',

		successfolderCreate: 'folder created',
		successUploadFile: 'file upload successed',
		successDelete: 'delete successed',
		successRename: 'rename successed',

		warningNotInCloudFolder: 'you cannot add new item in non cloud folder',
		warningCannotDisconnectStorage: 'you cannot disconnect storage',
		warningDelete: 'you cannot download from Dropbox',
	}
};