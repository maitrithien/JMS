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

        open = function (e) {
            
        }

        change = function (e) {
            
        }

        // Init object
        this.init = function () {
            // Init dropdownlist
            $(elem).kendoDropDownList({
                filter: "contains",
                width: settings.width,
                height: settings.height,
                dataTextField: settings.dataTextField,
                dataValueField: settings.dataValueField,
                dataSource: dataSource,
                change: (typeof settings.change == 'function') ? settings.change : change,
                open: (typeof settings.open == 'function') ? settings.open : open,
                index: settings.selectedIndex,

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
                localization: {
                    select: 'Chọn tập tin',
                    remove: 'Xóa',
                    cancel: 'Hủy',
                    dropFilesHere: 'Kéo thả tập tin vào đây',
                    headerStatusUploading: 'Đang tải lên...',
                    headerStatusUploaded: 'Hoàn tất',
                    retry: 'Thử lại',
                    statusUploaded: 'Đã tải lên thành công'
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

var common = new function () {
    this.getFormDataTypeJson = function (formId) {

        var elems = $('#' + formId).serializeArray();//.find('input, textarea, checkbox');
        console.log(elems);

        var data = {};
        $.each(elems, function (i, obj) {
            data[obj.name] = obj.value;
        });

        return data;
    };

    this.checkAll = function () {
        var checked = $('#chkAll').prop('checked');
        var chkList = $('#chkAll')
            .closest('.k-grid.k-widget')
            .find('.k-grid-content tr input[type=checkbox]');

        $.each(chkList, function (i, val) {
            $(val).prop('checked', checked);
        });
    }
};

$(document).ready(function () {
    kendo.culture("en-GB");
});