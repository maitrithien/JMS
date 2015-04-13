/// <reference path="common.js" />
/****************************************************************
                        ATTACHMENTS JS
*****************************************************************/

$(document).ready(function () {
    attachment.saveUrl = $('#saveUrl').val();
    attachment.removeUrl = $('#removeUrl').val();
    attachment.init();
});

// Attachments object
var attachment = new function () {
    var element = $("#files");

    this.saveUrl = null;
    this.removeUrl = null;
    this.autoUpload = false;

    var success = function (e) {
        data = [];
        if (e.files) {
            $.each(e.files, function (i, val) {
                data.push({
                    AttachmentFileName: e.files[i].name,
                    AttachmentFileSize: e.files[i].size,
                    AttachmentFileType: e.files[i].rawFile.type,
                    AttachmentFileExtension: e.files[i].extension
                });
            });
        }

        $.ajax({
            type: 'POST',
            url: $('#attachmentUrl').val(),
            data: data,
            dataType: 'json',
            success: onSuccess
        });
    };

    var onSuccess = function (data) {
        console.log(data);
    }

    this.init = function () {
        $("#files").uploader({
            saveUrl: attachment.saveUrl,
            removeUrl: attachment.removeUrl,
            autoUpload: attachment.autoUpload,
            success: success
        });
    };
};