window.cloud = window.cloud || {};

cloud.services = cloud.services || {};

cloud.services.loaderService =
	cloud.services.loaderService || function () {
		var loadersQueue = [];

		function isLoader() {
			return loadersQueue;
		}

		function show() {
			loadersQueue.push(0);
		};

		function remove() {
			if (loadersQueue.length) {
				loadersQueue.pop();
			}
		}

		return {
			isLoader: isLoader,
			show: show,
			remove: remove
		};
	};