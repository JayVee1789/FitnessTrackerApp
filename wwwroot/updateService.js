window.updateService = {
    register: function (onUpdateAvailable) {
        if ('serviceWorker' in navigator) {
            navigator.serviceWorker.register('service-worker.js')
                .then(reg => {
                    reg.onupdatefound = () => {
                        const installingWorker = reg.installing;
                        installingWorker.onstatechange = () => {
                            if (installingWorker.state === 'installed') {
                                if (navigator.serviceWorker.controller) {
                                    onUpdateAvailable.invokeMethodAsync('NotifyUpdateAvailable');
                                }
                            }
                        };
                    };
                });
        }
    },
    reloadApp: function () {
        window.location.reload(true);
    }
};
