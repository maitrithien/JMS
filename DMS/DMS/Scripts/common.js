(function ($) {
    /*---------------------------------------------------
                    DROPDOWN COMPONENT
    -----------------------------------------------------*/
    var Dropdown = function (element, options) {
        var elem = $(element);
        var obj = this;
        // Merge object data
        var settings = $.extend({
            width: '100%',
            height: 300,
            dataTextField: 'text',
            dataValueField: 'value',
            url: null,
            dataSource: {}
        }, options || {});

        var dataSource = (settings.url != null)
            ? {
                transport: {
                    read: {
                        dataType: 'json',
                        url: settings.url
                    }
                }
            }
            : dataSource;

        // Init object
        this.init = function () {
            // Init dropdownlist
            $(elem).kendoDropDownList({
                width: settings.width,
                height: settings.height,
                dataTextField: settings.dataTextField,
                dataValueField: settings.dataValueField,
                dataSource: settings.dataSource,
                index: settings.selectedIndex
            });

            return this;
        };

        return this.init();
    };
    $.fn.dropdownlist = function (options) {
        return this.each(function () {
            var element = $(this);

            // Return early if this element already has a plugin instance
            if (element.data('dropdownlist')) return;

            // pass options to plugin constructor
            var dropdown = new Dropdown(this, options);

            // Store plugin object in this element's data
            element.data('dropdownlist', dropdown);
        });
    };
    /*---------------------------------------------------
                    UPLOADER COMPONENT
    -----------------------------------------------------*/
    var Uploader = function (element, options) {
        var elem = $(element);
        var obj = this;
        // Merge object data
        var settings = $.extend({
            multiple: false,
            saveUrl: null,
            removeUrl: null,
            autoUpload: false,
            success: null
        }, options || {});

        // Init object
        this.init = function () {
            // Init uploader
            $(elem).kendoUpload({
                multiple: settings.multiple,
                async: {
                    saveUrl: settings.saveUrl,
                    removeUrl: settings.removeUrl,
                    autoUpload: settings.autoUpload
                },
                //files: initialFiles,
                success: settings.success
            });

            return this;
        };

        return this.init();
    };
    $.fn.uploader = function (options) {
        return this.each(function () {
            var element = $(this);

            // Return early if this element already has a plugin instance
            if (element.data('uploader')) return;

            // pass options to plugin constructor
            var uploader = new Uploader(this, options);

            // Store plugin object in this element's data
            element.data('uploader', uploader);
        });
    };

})(jQuery);