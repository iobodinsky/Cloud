window.cloud = window.cloud || {};

window.cloud.services = window.cloud.services || {};

window.cloud.services.folderHistoryService = function() {
    var folders = [];

    function addFolder(folder) {
        folders.push(folder);
    };

    function moveTo(folder) {
        for (var i = 0; i < folders.length; i++) {
            if (folders[i].id === folder.id) {
                folders.length = i;
            }
        }
    }

    function clearFolderHistory() {
        folders.length = 0;
    };

    return {
        folders: folders,
        addFolder: addFolder,
        moveTo: moveTo,
        clearFolderHistory: clearFolderHistory
    };
};