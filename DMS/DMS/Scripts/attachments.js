/// <reference path="common.js" />
/****************************************************************
                        ATTACHMENTS JS
*****************************************************************/

// Attachments object
var uploader = new function () {
    var element = $("#files");

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

            var history = $("#gridHistories").data("KendoGrid");
            if (history) {
                history.dataSource.read();
            }
        }
    }

    this.initFiles = function () {
        $("#files").uploader({
            saveUrl: $('#saveUrl').val(),
            removeUrl: $('#removeUrl').val(),
            autoUpload: false,
            multiple: true,
            success: success
        });
    };
};

$(document).ready(function () {
    uploader.initFiles();
});