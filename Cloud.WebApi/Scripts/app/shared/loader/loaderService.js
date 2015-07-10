window.cloud = window.cloud || {};

window.cloud.services = cloud.services || {};

window.cloud.services.loaderService = function() {
    var loadersQueue = [];

    function show() {
        loadersQueue.push(0);
    };

    function remove() {
        if (loadersQueue.length) {
            loadersQueue.pop();
        }
    }

    return {
        loaders: loadersQueue,
        show: show,
        remove: remove
    };
};