var User = {
    LoginUrl: undefined,
    AccountApiControllerUrl: undefined,

    UserId: undefined,
    Email: undefined,

    LastName: undefined,
    FirstName: undefined,
    PhoneNumber: undefined,
    PhoneNumberConfirmed: false,

    IsAuthenticated: function () {
        return User.UserId > 0;
    },

    importantSet_PhoneNumberConfirmed: function (value) {
        User.PhoneNumberConfirmed = value;
        localStorage.setItem("User.PhoneNumberConfirmed", User.PhoneNumberConfirmed);
    },

    loadInfo: function () {
        if (User.UserId == 0)
            return;

        if (Modernizr.localstorage) {
            User.LastName = localStorage.getItem("User.LastName");
            User.FirstName = localStorage.getItem("User.FirstName");
            User.PhoneNumber = localStorage.getItem("User.PhoneNumber");
            User.PhoneNumberConfirmed = localStorage.getItem("User.PhoneNumberConfirmed") === "true";

            if (User.LastName != null
                && User.FirstName != null
                && User.PhoneNumber != null) {
                return;//User data already loaded from store
            }
        }

        $.ajax({
            url: User.AccountApiControllerUrl + '/info',
            type: "GET",
            success: function (data) {
                User.setUserInfo(data);
            }
        });
    },

    setUserInfo: function (userData) {
        User.LastName = userData.LastName;
        User.FirstName = userData.FirstName;
        User.PhoneNumber = userData.PhoneNumber;
        User.PhoneNumberConfirmed = userData.PhoneNumberConfirmed;

        localStorage.setItem("User.LastName", User.LastName);
        localStorage.setItem("User.FirstName", User.FirstName);
        localStorage.setItem("User.PhoneNumber", User.PhoneNumber);
        localStorage.setItem("User.PhoneNumberConfirmed", User.PhoneNumberConfirmed);
    }
}