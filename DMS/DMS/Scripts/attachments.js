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
            console.log(e.files);
            $.each(e.files, function (i, val) {
                data.push({
                    AttachmentFileName: e.files[i].name,
                    AttachmentFileSize: e.files[i].size,
                    AttachmentFileType: e.files[i].rawFile.type,
                    AttachmentFilePath: e.files[i].rawFile.name,
                    AttachmentFileExtension: e.files[i].extension,
                    JobAPK: $('#JobAPK').val()
                });
            });
        }
        var isUpload = e.response == 0;

        $.ajax({
            type: 'POST',
            url: isUpload ? $('#addAttachmentsUrl').val() : $('#removeAttachmentsUrl').val(),
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            success: onSuccess
        });
    };

    var onSuccess = function (data) {
        var gridAttachment = $("#gridAttachments").data("kendoGrid");
        if (gridAttachment) {
            data = { JobAPK: $("#JobAPK").val() };
            gridAttachment.dataSource.read(data);
        }
    }

    this.init = function () {
        $("#files").uploader({
            saveUrl: attachment.saveUrl,
            removeUrl: attachment.removeUrl,
            autoUpload: attachment.autoUpload,
            multiple: true,
            success: success
        });
    };
};
