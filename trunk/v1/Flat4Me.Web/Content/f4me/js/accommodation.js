var Accommodation = {
    MIN_SECONDS_BEFORE_SEND_NEW_CODE: 30,
    AccommodationApiUrl: '',
    SearchApiUrl: '',

    flatMainListTemplate: undefined,
    flatTemplateTop: undefined,
    flatTemplateBody: undefined,    

    AccommodationSearchList: undefined,
    Accommodation: undefined,
    AccommodationId: undefined,
    AccommodationLocationId: undefined,
    CreateEditMap: undefined,
    SearchMap: undefined,
    CityUrl: undefined,
    CityId: undefined,
    CityPointY: undefined,
    CityPointX: undefined,
    CityZoom: undefined,

    ReservationId: undefined,

    init: function (options) {
        // Web api url
        this.AccommodationApiUrl = options.accommodationApiUrl || '';
        this.SearchApiUrl = options.searchApiUrl || '';

        this.flatMainListTemplate = Handlebars.templates.flatMainListTemplate;
        this.flatTemplateTop = Handlebars.templates.flatTemplateTop;
        this.flatTemplateBody = Handlebars.templates.flatTemplateBody;
        Handlebars.registerPartial("flatTopMenuTemplate", Handlebars.templates.flatTopMenuTemplate);
        Handlebars.registerPartial("flatPriceItemTemplate", Handlebars.templates.flatPriceItemTemplate);
    },

    loadForView: function (accommodationId) {
        this.AccommodationId = accommodationId;
        this.get(accommodationId, '#accommodationContainer').done(function (data) {
            // Set accommodation object
            Accommodation.Accommodation = data;

            $('#flatContainerTop').html('');
            $('#flatContainerBody').html('');
            $('#flatContainerTop').append(Accommodation.flatTemplateTop(Accommodation.Accommodation));
            $('#flatContainerBody').append(Accommodation.flatTemplateBody(Accommodation.Accommodation));

            Accommodation.initCarousel();
            // User has current reservation
            if (Accommodation.Accommodation.HasReservation) {
                Accommodation.ReservationId = Accommodation.Accommodation.CurrentReservation.ReservationId;
                $('#LastName').html(User.LastName);
                $('#FirstName').html(User.FirstName);
                $('#PhoneNumber').html(User.PhoneNumber);
                $('#Email').html(User.Email);
            }
            else {
                Accommodation.initReservationControl();
            }

            // Check reservation control position
            $('#reservationPanel').affix({
                offset: {
                    top: 700
                }
            });
            $('#reservationPanel').css('margin-top', '25px');
            $('#reservationPanel').css('top', '25px');
            $('#reservationPanel').affix('checkPosition');

            // initialize tooltips
            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="popover"]').popover({
                html: true,
                trigger: 'hover'
            });

            ymaps.ready(Accommodation.initYandexMapForView);
        });
    },

    initCarousel: function () {
        $('.carousel').carousel().on('slid.bs.carousel', function () {
            currentIndex = $('div.active').index();
            if (currentIndex == 0) {
                $('.carousel').carousel('pause');// Stop cycling when map has showed
            }
        });
        // Click on photo opens the image gallery
        $('.carousel .cover-photo').on('click', function (event) {
            // image index minus map
            currentIndex = $('div.active').index() - 1;
            Accommodation.showImageGallery(currentIndex);
        });
    },

    showImageGallery: function (index) {
        var photoElementList = $('.carousel .cover-photo');

        blueimp.Gallery(
            photoElementList, {
                urlProperty: 'img-path',
                titleProperty: 'img-title',
                index: index || 0
            });
    },

    initYandexMapForView: function () {
        var accommodation = Accommodation.Accommodation;
        if (!accommodation
            || !accommodation.Location) return;

        var pointY = accommodation.Location.PointY;
        var pointX = accommodation.Location.PointX;

        if (!pointY || !pointX) return;

        var defaultMapControls = "default";
        var pageWidth = $(document).width();
        // If page width small
        if (pageWidth < 400) {
            // Набор кнопок, предназначенный для маленьких карт.
            defaultMapControls = "smallMapDefaultSet";
        }

        // Init map
        var map = new ymaps.Map('mapContainer', {
            center: [pointY, pointX],
            zoom: 16,
            // плюс, дополнительно, кнопку построения маршрутов
            controls: [defaultMapControls, "routeEditor"]
        });

        // Поисковая панель
        var searchControl = map.controls.get('searchControl');
        // Поиск по народной карке (так получается найти кинотеатры, больницы и т.п.)
        searchControl.options.set('provider', 'yandex#publicMap');
        // Флаг, позволяющий учитывать при поиске границы видимой области карты. 
        // При значении true рассчитанная область видимости имеет больший приоритет, чем заданная через boundedBy.
        searchControl.options.set('useMapBounds', true);

        // Add accommodation to map
        var myPlacemark = new ymaps.Placemark([pointY, pointX], {
            balloonContent: accommodation.Name + ', ' + accommodation.StreetName + ', ' + accommodation.HouseNumber
        }, {
            preset: 'islands#dotIcon'
        });
        map.geoObjects.add(myPlacemark);

        Accommodation.CreateEditMap = map;
    },

    initYandexMapForSearch: function () {
        var pointY = Accommodation.CityPointY;
        var pointX = Accommodation.CityPointX;
        var zoom = Accommodation.CityZoom;

        if (!pointY || !pointX) return;

        var defaultMapControls = "default";
        var pageWidth = $(document).width();
        // If page width small
        if (pageWidth < 400) {
            // Набор кнопок, предназначенный для маленьких карт.
            defaultMapControls = "smallMapDefaultSet";
        }

        // Init map
        var map = new ymaps.Map('mapContainer', {
            center: [pointY, pointX],
            zoom: zoom,
            // плюс, дополнительно, кнопку построения маршрутов
            controls: [defaultMapControls, "routeEditor"]
        });

        // Поисковая панель
        var searchControl = map.controls.get('searchControl');
        // Поиск по народной карке (так получается найти кинотеатры, больницы и т.п.)
        searchControl.options.set('provider', 'yandex#publicMap');
        // Флаг, позволяющий учитывать при поиске границы видимой области карты. 
        // При значении true рассчитанная область видимости имеет больший приоритет, чем заданная через boundedBy.
        searchControl.options.set('useMapBounds', true);

        Accommodation.SearchMap = map;
        Accommodation.makeSearch();// search flats
    },

    makeSearch: function () {
        var checkin = $('#checkin').datepicker('getDate');
        var checkout = $('#checkout').datepicker('getDate');
        var guests = $('#guests').val();
        var minPrice = $('#minPrice').val();
        var maxPrice = $('#maxPrice').val();

        var criteria = new Object();
        criteria.CityId = Accommodation.CityId;
        criteria.Params = {
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            GuestsCount: guests,
            CheckInDate: checkin.toDateString(),
            CheckOutDate: checkout.toDateString()
        };
        var url = Accommodation.SearchApiUrl + '/find/';

        $.ajax({
            url: url,
            data: criteria,
            type: "POST",
            success: function (data) {
                Accommodation.AccommodationSearchList = data.AccommodationList;

                Accommodation.SearchMap.geoObjects.removeAll();
                ymaps.geoQuery(data.YaMapJson).addToMap(Accommodation.SearchMap);

                //.applyBoundsToMap(Accommodation.SearchMap, { checkZoomRange: true });

                var html = Accommodation.flatMainListTemplate(Accommodation.AccommodationSearchList);
                $('#accommodationListContainer').html('');
                $('#accommodationListContainer').append(html);
                // Make link to each accommodation
                $.each($('[data-link="accommodation"]'), function (ix, val) {
                    var accommodationId = $(val).data('accommodation-id');
                    if (accommodationId) {
                        var url = Accommodation.CityUrl + '/flat/' + accommodationId;
                        $(val).attr('href', url);
                    }
                });
            },
            error: function (data) {

            }
        });
    },

    getMainList: function (options) {
        Accommodation.CityPointY = options.cityPointY;
        Accommodation.CityPointX = options.cityPointX;
        Accommodation.CityZoom = options.cityZoom;
        Accommodation.CityUrl = options.cityUrl;
        Accommodation.CityId = options.cityId;

        // When map is ready, then load flats (flat's loading inside this function)
        ymaps.ready(Accommodation.initYandexMapForSearch);

        // Init price slider        
        var priceRangeElement = $("#priceRange")[0];
        noUiSlider.create(priceRangeElement, {
            connect: true,
            behaviour: 'drag-tap',
            start: [1000, 6000],
            step: 100,
            range: {
                'min': 1000,
                'max': 6000
            },
            pips: {
                mode: 'values',
                density: 4,
                values: [1000, 2000, 3000, 4000, 5000, 6000]
            }
        });
        // Set information for client about whish prices
        priceRangeElement.noUiSlider.on('update', function (values, handle) {
            if (handle == 0) {
                var minPrice = Number(values[0]);
                $('#minPrice').val(minPrice);
                $('#minPrice').change();
                $('#minPriceUI').text(minPrice);
            } else {
                var maxPrice = Number(values[1]);
                $('#maxPrice').val(maxPrice);
                $('#maxPrice').change();
                $('#maxPriceUI').text(maxPrice);
            }
        });

        // Init datepicker
        var today = new Date();
        var inputs = $('#searchDatepicker .actual_range').toArray();
        $('#searchDatepicker').datepicker({
            inputs: inputs,
            language: "ru",
            orientation: "auto left",
            startDate: today
        });
        // Set initial dates
        var tomorrow = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1, 0, 0, 0, 0);
        $('#checkin').datepicker('setDate', tomorrow);

        // Add to tomorrow MinDurationDays
        var minDurationDays = 1;
        var dateDeparture = new Date(tomorrow.getFullYear(), tomorrow.getMonth(), tomorrow.getDate() + minDurationDays, 0, 0, 0, 0);
        $('#checkout').datepicker('setDate', dateDeparture);

        // Register for search params changing
        $('#checkin').on('change', function () { Accommodation.onSearchParamsChanged(); });
        $('#checkout').on('change', function () { Accommodation.onSearchParamsChanged(); });
        $('#guests').on('change', function () { Accommodation.onSearchParamsChanged(); });
        $('#minPrice').on('change', function () { Accommodation.onSearchParamsChanged(); });
        $('#maxPrice').on('change', function () { Accommodation.onSearchParamsChanged(); });
    },

    lastTimerId: undefined,
    onSearchParamsChanged: function () {
        //
        if (Accommodation.lastTimerId) {
            clearTimeout(Accommodation.lastTimerId);
        }
        // Wait for really changed
        Accommodation.lastTimerId = setTimeout(function () {
            // Call search request to server
            Accommodation.makeSearch();
        }, 500);
    },

    get: function (accommodationId, blockElementSelector) {
        BlockUI.block($(blockElementSelector));
        var url = this.AccommodationApiUrl + '/' + accommodationId;

        return $.ajax({
            url: url,
            data: null,
            type: "GET",
            success: function (data) {
                BlockUI.unblock($(blockElementSelector));
            },
            error: function (data) {
                BlockUI.unblock($(blockElementSelector));
            }
        });
    },

    openFlat: function (flatId) {
        return Accommodation.CityUrl + '/flat/' + flatId;
    },

    // Right-side reservation control
    initReservationControl: function () {
        // set form data
        $('#AccommodationId').val(Accommodation.AccommodationId);

        // init datepicker
        var today = new Date();
        var inputs = $('#accommodationDatepicker .actual_range').toArray();
        $('#accommodationDatepicker').datepicker({
            inputs: inputs,
            language: "ru",
            orientation: "auto left",
            startDate: today
        }).on('changeDate', function (e) {
            // CALCULATE TOTAL DAYS WHEN DATE HAS CHANGED
            var checkin = $('#checkin').datepicker('getDate');
            if (!checkin) return;
            var checkout = $('#checkout').datepicker('getDate');
            if (!checkout) return;

            $('[name="Checkin"]').val(checkin.toDateString());
            $('[name="Checkout"]').val(checkout.toDateString());

            var totalDays = Accommodation.calculateTotalDays(checkin, checkout);
            var totalDaysText = '' + totalDays + ' ' + (totalDays == 1 ? 'сутки' : 'суток');
            $('#accommotationTotalDays').html(totalDaysText);

            var dayAmount = Accommodation.getDayAmount(totalDays);
            var maxDayAmount = Accommodation.getMaxDayAmount();

            var totalAmount = dayAmount * totalDays;

            $('#EstimatedAmount').text(totalAmount);
            $('[name="EstimatedAmount"]').val(totalAmount);

            var amountDetails = dayAmount + 'р' + ' x ' + totalDays;
            $('#AmountDetails').text(amountDetails);

            if (dayAmount < maxDayAmount) {
                $('#AmountWithoutDiscount').css('display', '');
                $('#AmountDiscount').css('display', '');

                var maxAmountDetails = maxDayAmount + 'р' + ' x ' + totalDays;
                var maxTotalAmount = maxDayAmount * totalDays;
                $('#AmountDetailsWithoutDiscount').text(maxAmountDetails);
                $('#EstimatedAmountWithoutDiscount').text(maxTotalAmount);

                $('#EstimatedAmountDiscount').text(maxTotalAmount - totalAmount);
            }
            else {
                $('#AmountWithoutDiscount').css('display', 'none');
                $('#AmountDiscount').css('display', 'none');
            }
        });

        var tomorrow = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1, 0, 0, 0, 0);
        $('#checkin').datepicker('setDate', tomorrow);

        // Add to tomorrow MinDurationDays
        var minDurationDays = $('#MinDurationDays').data('value');
        var dateDeparture = new Date(tomorrow.getFullYear(), tomorrow.getMonth(), tomorrow.getDate() + minDurationDays, 0, 0, 0, 0);
        $('#checkout').datepicker('setDate', dateDeparture);

        Accommodation.checkLocalReservation();
    },

    checkLocalReservation: function () {
        if (!Modernizr.localstorage) {
            return;
        }

        var checkin = localStorage.getItem("checkin");
        var checkout = localStorage.getItem("checkout");
        var guests = localStorage.getItem("guests");
        var children = localStorage.getItem("children");

        if (checkin == null || checkout == null || guests == null || children == null) {
            localStorage.clear();
            return;
        }

        // Check all data is ok
        if (checkin && checkout && guests && children) {
            $('#checkout').datepicker('setDate', new Date(checkout));
            $('#checkin').datepicker('setDate', new Date(checkin));
            $('[name="Guests"]').val(guests);
            $('[name="Children"]').val(children);

            if (User.IsAuthenticated() == true) {
                // Show confirmation
                Accommodation.makeReservation();
            }
        }
    },

    calculateTotalDays: function (checkin, checkout) {
        if (!checkin) {
            return "1 сутки";
        }
        if (!checkout) {
            return "1 сутки";
        }

        // Get 1 day in milliseconds
        var one_day = 1000 * 60 * 60 * 24;
        // Convert both dates to milliseconds
        var date1_ms = checkin.getTime();
        var date2_ms = checkout.getTime();
        // Calculate the difference in milliseconds
        var difference_ms = date2_ms - date1_ms;

        // Convert back to days and return
        var totalDays = Math.round(difference_ms / one_day);

        return totalDays;
    },

    getDayAmount: function (totalDays) {
        if (!totalDays) {
            return 0;
        }

        var amount = 0;
        $.each(Accommodation.Accommodation.PriceList, function (ix, vl) {
            if (totalDays >= vl.DurationDays) {
                amount = vl.Amount;
            }
        });

        return amount;
    },

    getMaxDayAmount: function () {
        var amount = null;
        $.each(Accommodation.Accommodation.PriceList, function (ix, vl) {
            if (amount == null) {
                amount = vl.Amount;
            }
            if (amount < vl.Amount) {
                amount = vl.Amount;
            }
        });

        return amount;
    },

    makeReservation: function () {
        if (User.IsAuthenticated() == false) {
            if (Modernizr.localstorage) {
                // Temporarily save reservation information
                var checkin = $('#checkin').datepicker('getDate');
                var checkout = $('#checkout').datepicker('getDate');
                var guests = $('[name="Guests"]').val();

                localStorage.setItem("checkin", checkin);
                localStorage.setItem("checkout", checkout);
                localStorage.setItem("guests", guests);
            }

            // Show reservation guest confirmation block
            $('#newGuestBlock').collapse();
            $('#reservationReservBtn').hide();

            return;
        }

        // Show reservation confirmation block
        $('#reservationConfirmationBlock').collapse();
        $('#reservationReservBtn').hide();
        // Set default form data

        $('#LastName').html(User.LastName);
        $('#FirstName').html(User.FirstName);
        $('#PhoneNumber').html(User.PhoneNumber);
        $('#Email').html(User.Email);
        // Scroll to btn
        $('html, body').animate({
            scrollTop: $("#reservationAcceptBtn").offset().top
        }, 2000);
    },

    // Confirm new guest
    confirmGuest: function () {
        var data = $('#reservationForm').serialize();

        var url = User.AccountApiControllerUrl + '/registerGuest';

        $.ajax({
            url: url,
            data: data,
            type: "POST",
            success: function (data) {
                User.setUserInfo(data);
                $('#newGuestBlock').collapse('hide');
                if (User.PhoneNumberConfirmed === true) {
                    Accommodation.confirmReservation();
                }
                else {
                    Accommodation.requestPhoneNumberConfirmation();
                }
            },
            statusCode: {
                401: function () {
                    toastr.info("Login");
                }
            },
            error: function (data) {
                Accommodation.checkErrorsOnGuestForm(data);                
            }
        });
    },

    checkErrorsOnGuestForm: function (data) {
        var validationResult = $.parseJSON(data.responseText);
        var modelState = validationResult.ModelState;
        // Check all fields errors
        if (modelState) {
            AjaxErrorHandler.errorHandled = true;

            // clear all errors
            $('.has-error').removeClass('has-error');

            $.each(modelState, function (fullFieldName, msg) {
                // fullFieldName include parameter name of guest.PhoneNumber POST function
                // Example: guest.PhoneNumber
                // Remove prefix
                var fieldName = fullFieldName.replace('guest.', '');
                var fieldSelector = '[name="' + fieldName + '"]';
                var field = $(fieldSelector);
                // Try find by id
                if (field.length === 0) {
                    fieldSelector = '[id="' + fieldName + '"]';
                    field = $(fieldSelector);
                }

                if (field.length === 0)
                    return;

                // Field might have a selector for choose other field
                var selector = $(field).data('selector');
                if (selector !== undefined) {
                    field = $(selector);
                }

                // Update tooltip with error message
                $(field).attr('data-original-title', msg);
                $(field).attr('data-toggle', 'tooltip');
                $(field).attr('data-placement', 'right');
                $(field).tooltip();

                // Indicate that field has error. Nice red border =)
                var parent = $(field).parent();
                if ($(parent).hasClass('has-error') == false)
                    $(parent).addClass('has-error');
            });

            toastr.error("В форме есть ошибки, исправьте, пожалуйста");
        }
    },

    requestPhoneNumberConfirmation: function (needServerRequest) {
        $('#phoneConfirmationBlock').collapse('show');
        if (needServerRequest && needServerRequest == true) {
            Accommodation.sendNewConfirmationCode();
        }
        Accommodation.runConfirmationTimer();
    },
    confirmPhoneNumber: function () {
        var code = $('#Code').val();
        if (!code || code == "") {
            toastr.warning("Напишите полученный код");
            return;
        }

        var url = User.AccountApiControllerUrl + '/confirmPhoneNumber/' + code;

        $.ajax({
            url: url,
            //data: code,
            type: "PUT",
            success: function (data) {
                if (data == true) {
                    User.importantSet_PhoneNumberConfirmed(true);
                    User.loadInfo();
                    $('#phoneConfirmationBlock').collapse('hide');
                    Accommodation.confirmReservation();
                }
                else {
                    toastr.error("Код не верный");
                }
            },
            error: function (data) {

            }
        });
    },

    confirmReservation: function () {
        if (User.PhoneNumberConfirmed == false) {
            $('#reservationConfirmationBlock').collapse('hide');
            Accommodation.requestPhoneNumberConfirmation(true);
            return;
        }
        var data = $('#reservationForm').serialize();
        if (Modernizr.localstorage) {
            localStorage.removeItem("checkin");
            localStorage.removeItem("checkout");
            localStorage.removeItem("guests");
        }

        var url = Accommodation.AccommodationApiUrl + '/addReservation';

        $.ajax({
            url: url,
            data: data,
            type: "POST",
            success: function (data) {
                Accommodation.loadForView(Accommodation.AccommodationId);
            },
            error: function (data) {

            }
        });
    },

    sendNewConfirmationCodeAlreadyRequested: false,
    sendNewConfirmationCode: function () {
        if (Accommodation.confirmationTimerSec > 0) {
            return;
        }
        if (Accommodation.sendNewConfirmationCodeAlreadyRequested == true) {
            return;
        }
        var url = User.AccountApiControllerUrl + '/requestConfirmationCode';

        Accommodation.sendNewConfirmationCodeAlreadyRequested = true;
        $('#sendNewConfirmationCodeSpin').show();
        $.ajax({
            url: url,
            data: null,
            type: "GET",
            complete: function () {
                Accommodation.sendNewConfirmationCodeAlreadyRequested = false;
                $('#sendNewConfirmationCodeSpin').hide();
            },
            success: function (data) {
                if (data == true) {
                    Accommodation.runConfirmationTimer();
                }
                else {
                    toastr.info("Невозможно отослать код. Попробуйте позже.");
                }
            },
            error: function (data) {
            }
        });
    },

    confirmationTimer: undefined,
    confirmationTimerSec: 0,
    runConfirmationTimer: function () {
        Accommodation.confirmationTimerSec = Accommodation.MIN_SECONDS_BEFORE_SEND_NEW_CODE;
        $('#sendNewCodeAfter').text(Accommodation.confirmationTimerSec);
        $('#sendNewCodeAfterBlock').show();

        Accommodation.confirmationTimer = setInterval(function () {
            Accommodation.confirmationTimerSec -= 1;
            $('#sendNewCodeAfter').text(Accommodation.confirmationTimerSec);

            if (Accommodation.confirmationTimerSec <= 0) {
                clearInterval(Accommodation.confirmationTimer);
                $('#sendNewCodeAfterBlock').hide();
            }
        }, 1000);
    },

    cancelReservation: function () {
        $.ajax({
            url: Accommodation.AccommodationApiUrl + '/cancelReservation/' + Accommodation.ReservationId,
            type: "PUT",
            success: function (data) {
                Accommodation.loadForView(Accommodation.AccommodationId);
            },
            error: function (data) {

            }
        });
    },
}