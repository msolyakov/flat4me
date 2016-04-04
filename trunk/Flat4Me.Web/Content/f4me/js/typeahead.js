/**
* Helper for initialize typeahed control (autocompletebox)
*/
var Typeahead = {
    /*
    Client side search
    */
    clientSideFor: function (ctrlSelector, keySelector, dataJsonRaw) {
        $(ctrlSelector).typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 0
        },
        {
            displayKey: 'Name',
            source: function (query, process) {
                var allItems = dataJsonRaw;
                var items = [];

                $.each(allItems, function (i, item) {
                    if (item.Name.toLowerCase().indexOf(query.trim().toLowerCase()) != -1) {
                        items.push(item);
                    }
                });

                process(items);
            }
        }).on('typeahead:selected', function (e, item) {
            $(keySelector).val(item.Id);
        }).on('typeahead:closed', function (e, item) {
            var a = $(ctrlSelector).val();
            if (a == '') {
                $(keySelector).val('');
            }
        })
    },
    /*
    Server side search
    */
    serverSideFor: function (ctrlSelector, keySelector, url, urlDataSelector) {
        urlDataSelector = urlDataSelector || function (query) {
            return { criteria: query };
        };

        $(ctrlSelector).typeahead(
		{
		    hint: false,
		    highlight: true,
		    minLength: 0
		},
		{
		    displayKey: 'Name',
		    source: function (query, process) {
		        var data = urlDataSelector(query);
		        $.ajax({
		            url: url,
		            data: data,
		            type: 'GET',
		            success: function (data) {
		                process(data);
		                if (keySelector && data.length != 0) $(keySelector).val(data[0].Id);
		                else $(keySelector).val(undefined);
		            }
		        });
		    },
		}).on('typeahead:selected', function (e, item) {
		    // set id field value
		    $(keySelector).val(item.Id);
		    // data- attributes
		    if (item.dataAttributes != undefined) {
		        $(keySelector).removeData();
		        $.each(item.dataAttributes, function (key, value) {
		            $(keySelector).data(key, value);
		        });
		    }
		    $(keySelector).change();
		}).on('typeahead:closed', function (e, item) {
		    var a = $(ctrlSelector).val();
		    // empty
		    if (a == '') {
		        // Clear key and attributes
		        $(keySelector).val('');
		        $(keySelector).removeData();
		        $(keySelector).change();
		    }
		});
    },
}