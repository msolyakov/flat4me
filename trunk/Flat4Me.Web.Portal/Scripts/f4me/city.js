var City = {
    CityUrl: undefined,

    init: function (options) {
        // controller action url
        this.CityUrl = options.cityUrl || '';
    },

    goToCity: function (cityId) {
        window.location = this.CityUrl + '/' + cityId;
    },
}