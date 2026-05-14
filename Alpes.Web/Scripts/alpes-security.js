(function () {
    "use strict";

    function getAntiForgeryToken() {
        var input = document.querySelector('input[name="__RequestVerificationToken"]');
        return input ? input.value : "";
    }

    function isUnsafeMethod(method) {
        var normalized = (method || "GET").toUpperCase();
        return normalized === "POST" || normalized === "PUT" || normalized === "PATCH" || normalized === "DELETE";
    }

    function isSameOrigin(url) {
        try {
            var parsed = new URL(url, window.location.href);
            return parsed.origin === window.location.origin;
        } catch (e) {
            return true;
        }
    }

    var originalFetch = window.fetch;

    if (originalFetch) {
        window.fetch = function (input, init) {
            init = init || {};

            var url = typeof input === "string" ? input : (input && input.url) || "";
            var method = (init.method || (input && input.method) || "GET").toUpperCase();

            if (isUnsafeMethod(method) && isSameOrigin(url)) {
                var headers = new Headers(init.headers || (input && input.headers) || {});
                var token = getAntiForgeryToken();

                if (token && !headers.has("RequestVerificationToken")) {
                    headers.set("RequestVerificationToken", token);
                }

                if (!headers.has("X-Requested-With")) {
                    headers.set("X-Requested-With", "XMLHttpRequest");
                }

                init.headers = headers;
                init.credentials = init.credentials || "same-origin";
            }

            return originalFetch(input, init);
        };
    }

    function configureJQueryAjax() {
        if (!window.jQuery) {
            return;
        }

        window.jQuery.ajaxPrefilter(function (options, originalOptions, jqXHR) {
            var method = (options.type || options.method || "GET").toUpperCase();

            if (isUnsafeMethod(method) && isSameOrigin(options.url || "")) {
                var token = getAntiForgeryToken();

                if (token) {
                    jqXHR.setRequestHeader("RequestVerificationToken", token);
                }

                jqXHR.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            }
        });
    }

    configureJQueryAjax();

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", configureJQueryAjax);
    }
}());
