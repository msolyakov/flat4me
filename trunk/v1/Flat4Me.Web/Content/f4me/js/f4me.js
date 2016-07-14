Date.prototype.toLocaleFormat = function (format) {
    var f = { y: this.getYear() + 1900, m: this.getMonth() + 1, d: this.getDate(), H: this.getHours(), M: this.getMinutes(), S: this.getSeconds() }
    for (var k in f)
        format = format.replace('%' + k, f[k] < 10 ? "0" + f[k] : f[k]);
    return format;
};

/*
* Global common
*/
var F4Me = {
    parseDateTimeToLocal: function (value) {
        var date = new Date(value).toLocaleFormat('%d.%m.%y %H:%M:%S');
        return date;
    },

    strictInputOnlyNumbers: function (selector) {
        $(selector).keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and . , 190, 188
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    },

    // make option selected by selected-value data attribute. If available
    makeOptionSelected: function (selector) {
        $.each($(selector), function (ix, val) {
            var selValue = $(val).data('selected-value');
            if (selValue) {
                $(val).find('option[value="' + selValue + '"]').attr('selected', 'selected');
            }
        });
    },

    // make all option selected by selected-value data attribute. If available
    makeAllOptionSelected: function () {
        $.each($('select'), function (ix, val) {
            var selValue = $(val).data('selected-value');
            if (selValue) {
                $(val).find('option[value="' + selValue + '"]').attr('selected', 'selected');
            }
        });
    },

    makeCheckBoxAsBootstrapYesNo: function (selector) {
        $(selector).bootstrapSwitch("destroy");
        $(selector).bootstrapSwitch({
            onColor: "success",
            onText: "Да",
            offText: "Нет"
        });
    },

    makeAllCheckBoxAsBootstrapYesNo: function () {
        this.makeCheckBoxAsBootstrapYesNo('input[type=checkbox]');
    },

    makeAllCheckBoxCheckedByClass: function () {
        $('input[type="checkbox"]').each(function () {
            if ($(this).hasClass('checked')) {
                $(this).attr('checked', true);
            }
        });
    }
}