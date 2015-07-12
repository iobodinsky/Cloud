window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || { };

window.cloud.services.constants = {
    userTokenKey: 'CloudUserBearerToken',
    userTokenType: 'Bearer',
    storages: {
        googleDriveId: 1,
        dropboxId: 3
    },
    rootCloudFolderId: 'cloudRoot',
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
            constructDisconnect: function(storageId) {
                return 'api/storages/' + storageId + '/disconnect';
            },
            files: {
                constructUpload: function(folderId, storageId) {
                    return 'api/files/storage/' + storageId + '/folder/' + folderId + '/upload';
                },
                constructRename: function(fileId, storageId) {
                    return 'api/files/' + fileId + '/storage/' + storageId + '/rename';
                },
                constructDelete: function(fileId, storageId) {
                    return 'api/files/' + fileId + '/storage/' + storageId + '/delete';
                },
            },
            folders: {
                rootFolderData: 'api/folders',
                constructCreate: function(storageId) {
                    return 'api/folders/storage/' + storageId + '/create';
                },
                constructRename: function(folderId, storageId) {
                    return 'api/folders/' + folderId + '/storage/' + storageId + '/rename';
                },
                constructFolderData: function(folderId, storageId) {
                    return 'api/folders/' + folderId + '/storage/' + storageId;
                },
                constructDelete: function(folderId, storageId) {
                    return 'api/folders/' + folderId + '/storage/' + storageId + '/delete';
                }
            },
        },
        drive: {
            authorize: 'api/storages/authorize/googledrive'
        },
        dropbox: {
            authorize: 'api/storages/authorize/dropbox',
            constructDownload: function(fileId) {
                return 'api/files/' + fileId + '/download/dropbox';
            }
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
        failGetStorages: 'fail to get storages',
        failCreatFolder: 'fail to create folder',
        failRename: 'fail to rename',
        failRegister: 'fail to register',
        failLogin: 'fail to login',
        failRequestDownloadLink: 'fail to download file',
        failCloudNotFound: 'cloud not found',
        failDelete: 'fail to delete',
        failDeleteEntityNotSupported: 'fail to delete this type entity',
        failRenameEntityNotSupported: 'fail to rename this type entity',
        failConnectToUnknownStorage: 'fail to connect to unknown storage',

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