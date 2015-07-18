window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || { };

window.cloud.services.constants = {
    userTokenKey: 'CloudUserBearerToken',
    userTokenType: 'Bearer',
    storages: {
        googleDriveAlias: 'googledrive',
        dropboxAlias: 'dropbox'
    },
    rootCloudFolderId: 'cloudRoot',
    rootCloudFolderName: 'Cloud',
    alert: {
        timeout: 4000,
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
    routeState: {
        login: 'login',
        register: 'register',
        storages: 'storages',
        connect: 'connect'
    },
    viewTemplatePath: {
        login: 'scripts/app/components/login/loginView.html',
        register: 'scripts/app/components/register/registerView.html',
        folder: 'scripts/app/components/folder/folderView.html',
        folderHistory: 'scripts/app/components/folderHistory/folderHistoryView.html',
        userAccount: 'scripts/app/components/userAccount/userAccountView.html',
        storage: 'scripts/app/components/storage/storageView.html',
        upload: 'scripts/app/components/upload/uploadView.html',
        manageStorages: 'scripts/app/components/modals/manageStorages/manageStoragesView.html',
        rename: 'scripts/app/components/modals/rename/renameView.html',
        createFolder: 'scripts/app/components/modals/createFolder/createFolderView.html',
        deleteConfirm: 'scripts/app/components/modals/deleteConfirm/deleteConfirmView.html',
        alert: 'scripts/app/shared/alert/alertView.html',
        loader: 'scripts/app/shared/loader/loaderView.html'
    },
    urls: {
        cloud: {
            home: '',
            token: '/Token',
            logout: 'api/Account/Logout',
            register: 'api/Account/Register',
            userInfo: 'api/Account/UserInfo',
            storages: 'api/storages',
            constructDisconnect: function(storage) {
                return 'api/storages/' + storage + '/disconnect';
            },
            files: {
                constructUpload: function(folderId, storage) {
                    return 'api/files/storages/' + storage + '/folder/' + folderId + '/upload';
                },
                constructRename: function(fileId, storage) {
                    return 'api/files/' + fileId + '/storages/' + storage + '/rename';
                },
                constructDelete: function(fileId, storage) {
                    return 'api/files/' + fileId + '/storages/' + storage + '/delete';
                },
            },
            folders: {
                rootFolderData: 'api/folders',
                constructCreate: function(storage) {
                    return 'api/folders/storages/' + storage + '/create';
                },
                constructRename: function(folderId, storage) {
                    return 'api/folders/' + folderId + '/storages/' + storage + '/rename';
                },
                constructFolderData: function(folderId, storage) {
                    return 'api/folders/' + folderId + '/storages/' + storage;
                },
                constructDelete: function(folderId, storage) {
                    return 'api/folders/' + folderId + '/storages/' + storage + '/delete';
                }
            },
        },
        drive: {
            authorize: 'api/storages/googledrive/authorize'
        },
        dropbox: {
            authorize: 'api/storages/dropbox/authorize',
            constructDownload: function(fileId) {
                return 'api/files/' + fileId + '/dropbox/download';
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
        failConnectToStorage: 'fail to connect to storage',
        failConnectToUnknownStorage: 'fail to connect to unknown storage',
        failDisconnectFromStorage: 'fail to disconnect from storage',

        infoDriveFileDownloadingNotAllowed: 'download file not allowed',
        infoDropboxDownloadNotAllowed: 'download file from Dropbox not allowed',

        successfolderCreate: 'folder created',
        successUploadFile: 'file upload successed',
        successDelete: 'delete successed',
        successRename: 'rename successed',

        warningNotInCloudFolder: 'you cannot add new item in non cloud folder',
        warningCannotDisconnectStorage: 'you cannot disconnect storage',
        warningDelete: 'you cannot download from Dropbox',
        warningChooseStorage: 'choose storage first'
    }
};