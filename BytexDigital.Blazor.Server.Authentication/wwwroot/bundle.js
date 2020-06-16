window.BytexDigitalAuthCookies = {
    setUserIdentifier: function (cookieString) {
        console.log(cookieString);
        document.cookie = cookieString;
    },
    removeUserIdentifier: function (cookieName) {
        document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    },
    getUserIdentifier: function (cookieName) {
        var cookieMatch = document.cookie.match(new RegExp('(^| )' + cookieName + '=([^;]+)'));

        if (cookieMatch) {
            return cookieMatch[2];
        } else {
            return null;
        }
    }
};