var Accommodation = {
    AccommodationIndexUrl: '',
    AccommodationApiUrl: '',
    PhotoApiUrl: '',
    CitySearchUrl: '',
    ShortMainListTemplate: undefined,
    ShortUserListTemplate: undefined,
    ShortItemViewTemplate: undefined,
    ShortItemEditTemplate: undefined,
    PriceListItemTemplate: undefined,
    PhotoListItemTemplate: undefined,
    ReservationListTemplate: undefined,
    Accommodation: undefined,
    AccommodationId: undefined,
    AccommodationLocationId: undefined,
    PrimaryPhoto: undefined,
    CreateEditMap: undefined,
    IsNew: false,//Takes action in save function.

    init: function (options) {
        // controller action url
        this.AccommodationIndexUrl = options.accommodationIndexUrl || '';
        // Web api url
        this.AccommodationApiUrl = options.accommodationApiUrl || '';
        this.PhotoApiUrl = options.photoApiUrl || '';
        this.CitySearchUrl = options.citySearchUrl || '';

        // ShortMainList
        if ($('#accommodationShortMainListTemplate').length == 1) {
            t = $('#accommodationShortMainListTemplate').html();
            this.ShortMainListTemplate = Handlebars.compile(t);
        }
        // ShortItemView
        if ($('#accommodationShortItemViewTemplate').length == 1) {
            t = $('#accommodationShortItemViewTemplate').html();
            this.ShortItemViewTemplate = Handlebars.compile(t);
        }
        if ($('#accommodationShortItemView_TopMenu_Template').length == 1) {
            t = $('#accommodationShortItemView_TopMenu_Template').html();// use only as partial
            Handlebars.registerPartial("accommodationShortItemView_TopMenu_Template", t);
        }
        // ShortUserList
        t = $('#accommodationShortUserListTemplate').html();
        this.ShortUserListTemplate = Handlebars.compile(t);
        // ShortItemEdit
        t = $('#accommodationShortItemEditTemplate').html();
        this.ShortItemEditTemplate = Handlebars.compile(t);
        // PriceListItemTemplate
        t = $('#priceListItemTemplate').html();
        this.PriceListItemTemplate = Handlebars.compile(t);
        Handlebars.registerPartial("priceListItemTemplate", t);
        // PhotoListItemTemplate
        t = $('#photoListItemTemplate').html();
        this.PhotoListItemTemplate = Handlebars.compile(t);
        Handlebars.registerPartial("photoListItemTemplate", t);
        // ReservationListTemplate
        t = $('#reservationListTemplate').html();
        this.ReservationListTemplate = Handlebars.compile(t);
    },

    initCreateEditControls: function () {
        if (Accommodation.IsNew) {
            Typeahead.serverSideFor("#cityTypeahead", "#CityId", this.CitySearchUrl);
        }
        else {
            // Location updatng is not allowed
            $('#cityTypeahead').prop('disabled', true);
            $('#Name').prop('disabled', true);
            $('#StreetName').prop('disabled', true);
            $('#HouseNumber').prop('disabled', true);
        }

        //F4Me.makeAllCheckBoxAsBootstrapYesNo();        
        F4Me.strictInputOnlyNumbers('[name=Area]');
        F4Me.strictInputOnlyNumbers('[name=Deposit]');
        F4Me.strictInputOnlyNumbers('#PriceList [name$=".Amount"]');
        // When edit it makes selected duration in price list
        // When create it does nothing
        F4Me.makeAllOptionSelected();
        // Make checkboxes checked by class
        F4Me.makeAllCheckBoxCheckedByClass();

        this.initImageUpload();
    },


    loadForCreate: function () {
        this.IsNew = true;
        Accommodation.addDraft()
            .done(function (data) {
                // Generate html by template
                var html = Accommodation.ShortItemEditTemplate(data);
                $('#createPageContainer').append(html);

                Accommodation.AccommodationId = data.AccommodationId;
                $('#CityId').val(User.CityId);
                var fullCityName = User.RegionName + ', ' + User.CityName;
                $('#cityTypeahead').val(fullCityName);

                Accommodation.initCreateEditControls();
                ymaps.ready(Accommodation.initYandexMapForCreate);
                $('[data-toggle="tooltip"]').tooltip();
            });
    },

    loadForEdit: function (accommodationId) {
        this.IsNew = false;
        this.AccommodationId = accommodationId;
        // there is no accommodation to edit
        if (!this.AccommodationId) {
            window.location = Accommodation.AccommodationIndexUrl;
            return;
        }

        this.get(accommodationId, '#accommodationContainer')
            .done(function (data) {
                // Generate html by template
                var html = Accommodation.ShortItemEditTemplate(data);
                $('#createPageContainer').append(html);

                Accommodation.PrimaryPhoto = $('img[data-primary="true"]')[0];

                Accommodation.initCreateEditControls();
                ymaps.ready(Accommodation.initYandexMapForEdit);
            });
    },

    loadForView: function (accommodationId) {
        this.AccommodationId = accommodationId;
        this.get(accommodationId, '#accommodationContainer').done(function (data) {
            // Set accommodation object
            Accommodation.Accommodation = data;
            // Generate html by template
            var html = Accommodation.ShortItemViewTemplate(data);
            $('#accommodationContainer').append(html);

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
            // initialize tooltips
            $('[data-toggle="tooltip"]').tooltip();

            ymaps.ready(Accommodation.initYandexMapForView);
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

    initImageUpload: function () {
        $('#imgUploadForm').fileupload({
            sequentialUploads: false,
            start: function (e, data) {
                $('.progress').show();
            },
            stop: function (e, data) {
                $('.progress').hide();
                $('.progress .progress-bar').css('width', '0%');
            },
            add: function (e, data) {
                $.each(data.files, function (index, file) {
                    loadImage(file, function (img) {
                        Accommodation.addImgToTable(img.src, file.name);
                    }, {
                        maxWidth: 140, maxHeight: 140
                    });
                });

                var photoUploadUrl = Accommodation.PhotoApiUrl + '/' + $('#AccommodationId').val();
                $('#imgUploadForm').fileupload({
                    url: photoUploadUrl
                });
                data.submit();
            },
            done: function (e, data) {
                // Update IMG element with PhotoId data attribute returned from server.
                $.each(data.result, function (index, item) {
                    // Find all photos with name (might be a few)
                    var photoElementList = $('#PhotoList [data-file-name="' + item.FileName + '"]');
                    $.each(photoElementList, function (index, photo) {
                        var photoId = $(photo).data('photo-id');
                        // Only when photo-id is not setted
                        if (photoId === undefined || photoId == '') {
                            // Set photo id
                            $(photo).data('photo-id', item.PhotoId);
                            // Add action onclick
                            $(photo).on("click", function () {
                                Accommodation.setPhotoPrimary($(photo));
                            });
                        }
                    });
                });
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('.progress .progress-bar').css('width', progress + '%');
                $('.progress .text-info').text(progress + '%');
            }
        });
    },

    initYandexMapForCreate: function () {
        var pointY = User.CityPointY || 53.195533; // Samara
        var pointX = User.CityPointX || 50.101801; // Samara        
        var zoom = User.CityZoom || 12;
        // Init Yandex map
        Accommodation.CreateEditMap = new ymaps.Map('mapContainer', {
            center: [pointY, pointX],
            zoom: zoom
        });

        // Listen when user choose
        Accommodation.CreateEditMap.geoObjects.events.add('click', function (e) {
            e.preventDefault();
            // Получение координат щелчка
            var t = e.get('target');
            var locationId = t.properties.get('locationId');
            var fullAddress = t.properties.get('balloonContent');
            Accommodation.confirmLocation(locationId).success(function () {
                Accommodation.AccommodationLocationId = locationId;
                toastr.info('Подтвержден: ' + fullAddress);
            });
        });

        // Watch for address fields
        $('#cityTypeahead').focusout(Accommodation.onAddressChanged);
        $('[name="StreetName"]').focusout(Accommodation.onAddressChanged);
        $('[name="HouseNumber"]').focusout(Accommodation.onAddressChanged);
    },

    initYandexMapForEdit: function () {
        // First init map by user city
        var pointY = User.CityPointY || 53.195533; // Samara
        var pointX = User.CityPointX || 50.101801; // Samara        
        var zoom = User.CityZoom || 12;
        // Init Yandex map
        Accommodation.CreateEditMap = new ymaps.Map('mapContainer', {
            center: [pointY, pointX],
            zoom: zoom
        });

        // Then Get accommodation coordinates
        $.ajax({
            url: Map.MapApiUrl + '/' + Accommodation.AccommodationId,
            type: "GET",
            success: function (data) {
                Accommodation.CreateEditMap.geoObjects.removeAll();

                var geoObjects = ymaps.geoQuery(data)
                .addToMap(Accommodation.CreateEditMap)
                .applyBoundsToMap(Accommodation.CreateEditMap, { checkZoomRange: true });
            }
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

        //// Результаты поиска будем помещать в коллекцию.
        //var mySearchResults = new ymaps.GeoObjectCollection(null, {
        //    hintContentLayout: ymaps.templateLayoutFactory.createClass('$[properties.name]')
        //});
        //map.geoObjects.add(mySearchResults);
        //// При клике по найденному объекту метка становится красной.
        //mySearchResults.events.add('click', function (e) {
        //    e.get('target').options.set('preset', 'islands#redIcon');
        //});
        //// Выбранный результат помещаем в коллекцию.
        //searchControl.events.add('resultselect', function (e) {
        //    var index = e.get('index');
        //    searchControl.getResult(index).then(function (res) {
        //        mySearchResults.add(res);
        //    });
        //}).add('submit', function () {
        //    mySearchResults.removeAll();
        //})

        // Add accommodation to map
        var myPlacemark = new ymaps.Placemark([pointY, pointX], {
            balloonContent: accommodation.Name + ', ' + accommodation.StreetName + ', ' + accommodation.HouseNumber
        }, {
            preset: 'islands#dotIcon'
        });
        map.geoObjects.add(myPlacemark);

        Accommodation.CreateEditMap = map;
    },

    onAddressChanged: function () {
        if (Accommodation.AccommodationId == undefined)
            return;
        // If any address field is in editing - return;
        if ($('#cityTypeahead').is(':focus'))
            return;
        if ($('#StreetName').is(':focus'))
            return;
        if ($('#HouseNumber').is(':focus'))
            return;

        // User has finished edit address fields
        var cityName = $('#cityTypeahead').val();
        var streetName = $('#StreetName').val();
        var houseNumber = $('#HouseNumber').val();
        // Get previous values
        var previousCityName = $('#cityTypeahead').data('previous-value');
        var previousStreetName = $('#StreetName').data('previous-value');
        var previousHouseNumber = $('#HouseNumber').data('previous-value');
        // Check if has not changed
        if (cityName == previousCityName
            && streetName == previousStreetName
            && houseNumber == previousHouseNumber) {
            return;
        }
        // Save previous values
        $('#cityTypeahead').data('previous-value', cityName);
        $('#StreetName').data('previous-value', streetName);
        $('#HouseNumber').data('previous-value', houseNumber);

        Accommodation.locateAccommodation(cityName, streetName, houseNumber);
    },

    locateAccommodation: function (cityName, streetName, houseNumber) {
        // Required parameters has setted
        if (!cityName || !streetName || !houseNumber)
            return;

        $.ajax({
            url: Map.MapApiUrl + '/Geocode',
            data: {
                AccommodationId: Accommodation.AccommodationId,
                CityName: cityName,
                StreetName: streetName,
                HouseNumber: houseNumber
            },
            type: "PUT",
            success: function (data) {
                // Clear all point on map
                Accommodation.CreateEditMap.geoObjects.removeAll();
                // Add point to map
                var geoObjects = ymaps.geoQuery(data)
                .addToMap(Accommodation.CreateEditMap)
                .applyBoundsToMap(Accommodation.CreateEditMap, { checkZoomRange: true });

                // try get LocationId
                var obj = $.parseJSON(data);
                // There is one location of accommodation
                if (obj && obj.features && obj.features.length == 1) {
                    // Set one location as primary
                    var locationId = $.parseJSON(data).features[0].LocationId;
                    Accommodation.confirmLocation(locationId);
                }
                    // There is no location
                else {
                    // Clear current location. 
                    // Might be in case when location was not found or there are more that one locations (user should choose it)
                    Accommodation.AccommodationLocationId = undefined;
                }
                // There are more that one location (user should choose it)
                if (obj && obj.features && obj.features.length > 1) {
                    toastr.warning('Найдено несколько объектов на карте. Пожалуйста, выбирите мышкой правильный.');
                }
            }
        });
    },

    confirmLocation: function (locationId) {
        // Confirm Location
        return $.ajax({
            url: Map.MapApiUrl + '/Confirm/' + locationId,
            type: "PUT"
        });
    },

    addImgToTable: function (imgSrc, fileName, photoId) {
        imgSrc = imgSrc || '';
        fileName = fileName || '';
        photoId = photoId || '';

        var data = {
            IsPrimary: false,
            TinyPath: imgSrc,
            PhotoId: photoId,
            FileName: fileName
        };
        var html = Accommodation.PhotoListItemTemplate(data);
        $('#PhotoList').append(html);
    },

    addPrice: function () {
        var currentMaxIndex = $('[name$="].ViewIndex"]').last().val() || -1;
        var data = { ViewIndex: ++currentMaxIndex };
        var html = Accommodation.PriceListItemTemplate(data);
        $('#PriceList').append(html);

        F4Me.strictInputOnlyNumbers('#PriceList [name$="Amount"]');
    },

    removePrice: function (viewIndex) {
        $('#PriceItem_' + viewIndex).remove();


    },

    getMainList: function (options) {
        if (!options.resultContainerSelector)
            return;

        if (options.blockElementSelector) BlockUI.block($(options.blockElementSelector));

        var url = this.AccommodationApiUrl;
        if (options.userId !== undefined) {
            url += '/user/' + options.userId;
        }
        else if (options.cityId !== undefined) {
            url += '/city/' + options.cityId;
        }
        var listTemplate = options.listTemplate || Accommodation.ShortMainListTemplate;

        $.ajax({
            url: url,
            data: null,
            type: "GET",
            success: function (data) {
                if (options.blockElementSelector) BlockUI.unblock($(options.blockElementSelector));

                var html = listTemplate(data);
                $(options.resultContainerSelector).append(html);
            },
            error: function (data) {
                if (options.blockElementSelector) BlockUI.unblock($(options.blockElementSelector));
            }
        });
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

    addDraft: function () {
        var blockElementSelector = '#createPageContainer';
        BlockUI.block($(blockElementSelector));
        return $.ajax({
            url: Accommodation.AccommodationApiUrl + '/addDraft',
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

    completeDraft: function () {
        if (!Accommodation.AccommodationLocationId) {
            toastr.error('Необходимо подтвердить адрес.')
            return;
        }

        var blockElementSelector = '#createPageContainer';
        BlockUI.block($(blockElementSelector));

        // Save accommodation
        var data = Accommodation.getFormData();
        $.ajax({
            url: Accommodation.AccommodationApiUrl + '/completeDraft',
            data: data,
            type: "POST",
            success: function (data) {
                window.location = Accommodation.AccommodationIndexUrl;
            },
            error: function (data) {
                BlockUI.unblock($(blockElementSelector));
                Accommodation.checkErrorsOnSave(data);
            }
        });
    },

    post: function () {
        var blockElementSelector = '#createPageContainer';
        BlockUI.block($(blockElementSelector));
        var data = this.getFormData();

        return $.ajax({
            url: Accommodation.AccommodationApiUrl + '/update',
            data: data,
            type: "POST",
            success: function (data) {
                window.location.replace(Accommodation.AccommodationIndexUrl);
            },
            error: function (data) {
                BlockUI.unblock($(blockElementSelector));
                Accommodation.checkErrorsOnSave(data);
            }
        });
    },

    save: function () {
        if (!Accommodation.PrimaryPhoto) {
            toastr.error('Необходимо выбрать главное фото.')
            return;
        }
        // if new then complete draft, else update accomodation
        return this.IsNew ? this.completeDraft() : this.post();
    },

    setPhotoPrimary: function (photo) {
        var alreadyPrimary = $(photo).data('primary');
        if (alreadyPrimary == true) {
            return;
        }
        var photoId = $(photo).data('photo-id');
        if (photoId == undefined || photoId == '') {
            return;
        }

        var photoContainer = $(photo).parent();
        BlockUI.block(photoContainer);

        return $.ajax({
            url: Accommodation.PhotoApiUrl + '/primary/' + photoId,
            type: "PUT",
            success: function (data) {
                BlockUI.unblock(photoContainer);
                $('#PhotoList .img-thumbnail.bg-primary').removeClass('bg-primary');
                $('#PhotoList .img-thumbnail').data('primary', false);
                $(photo).addClass('bg-primary');
                $(photo).data('primary', true);
                Accommodation.PrimaryPhoto = photo;
            },
            error: function (data) {
                BlockUI.unblock(photoContainer);
            }
        });
    },

    removePhoto: function (photo) {
        var photoId = $(photo).data('photo-id');
        if (photoId == undefined || photoId == '') {
            return;
        }

        var photoContainer = $(photo).parent();
        BlockUI.block(photoContainer);

        return $.ajax({
            url: Accommodation.PhotoApiUrl + '/' + photoId,
            type: "DELETE",
            success: function (data) {
                BlockUI.unblock(photoContainer);
                $(photoContainer).remove();
            },
            error: function (data) {
                BlockUI.unblock(photoContainer);
            }
        });
    },

    checkErrorsOnSave: function (data) {
        var validationResult = $.parseJSON(data.responseText);
        var modelState = validationResult.ModelState;
        // Check all fields errors
        if (modelState) {
            AjaxErrorHandler.errorHandled = true;

            // clear all errors
            $('.has-error').removeClass('has-error');

            $.each(modelState, function (fullFieldName, msg) {
                // fullFieldName include parameter name of Accommodation.AccommodationApiUrl POST function
                // Example: accommodation.Name
                // Remove prefix
                var fieldName = fullFieldName.replace('accommodation.', '');
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

    getFormData: function () {
        var data = $('#accommodationDataForm').serializeArray();

        // Normalize PriceList array START. 
        // Migth be PriceList[0].DurationDays, PriceList[2].DurationDays <-- wrong index order. In this case server miss PriceList[2]
        // Shoul be PriceList[0].DurationDays, PriceList[1].DurationDays
        var priceListIx = -1;
        var previousPriceListIx = undefined;
        $.each(data, function (ix, val) {
            // name starts with
            if (val.name.indexOf('PriceList') == 0) {
                var arrayBracketStartIx = val.name.indexOf('[') + 1;
                var arrayBracketEndIx = val.name.indexOf(']');
                var currentIx = val.name.substring(arrayBracketStartIx, arrayBracketEndIx);
                if (currentIx != previousPriceListIx) {
                    previousPriceListIx = currentIx;
                    priceListIx++;
                }

                val.name = val.name.replace('[' + currentIx + ']', '[' + priceListIx + ']');
            }
        });
        // Normalize PriceList array END

        return data;
    },

    getReservationList: function (accommodationId, checkinStart) {
        //this.getReservationList(accommodationId, new Date(2015, 3, 1).toJSON());
        var url = this.AccommodationApiUrl + '/reservationList';

        return $.ajax({
            url: url,
            data: {
                AccommodationId: accommodationId,
                CheckinStart: checkinStart
            },
            type: "PUT"
        });
    },

    loadReservationList: function (accommodationId, checkinStart) {
        this.getReservationList(accommodationId, checkinStart).success(function (data) {
            var html = Accommodation.ReservationListTemplate(data);
            $('#reservationListContainer').append(html);
        });
    },
}