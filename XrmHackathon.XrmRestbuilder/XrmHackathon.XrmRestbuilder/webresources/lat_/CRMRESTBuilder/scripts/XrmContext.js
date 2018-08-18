Xrm = window.Xrm || { __namespace: true };
Xrm.Page = Xrm.Page || { __namespace: true };
Xrm.Page.context = {
    getClientUrl: function () {
        return window.SERVER_URL;
    },
    getUserLcid: function () {
        return 1033;
    }
}
Xrm.Utility = Xrm.Utility|| { __namespace: true };
Xrm.Utility = {
    alertDialog: function (message) {
        alert(message);
    }
}