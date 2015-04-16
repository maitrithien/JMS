$(document).ready(function () {
    jobs.viewer.initAttachmentGrid();
    jobs.viewer.initAttachmentDeleteConfirmDialog();
});

var jobs = new function () {
    this.viewer = new function () {
        var attachmentConfirmTemplate = null;
        var attachmentConfirmWindow = null;

        function updateAttachment_Click(e) {
            e.preventDefault();

            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            var win = $("#UpdateAttachmentDialog").data("kendoWindow");
            win.center();
            win.refresh({
                url: $('#updateAttachmentDialogUrl').val(),
                data: { apk: dataItem.APK },
                type: 'POST'
            });
            win.open();
        }
        this.closeUpdateAttachmentDialog = function () {
            var win = $("#UpdateAttachmentDialog").data("kendoWindow");
            if (win) {
                win.close();
            }
        };

        this.refreshAttachmentGrid = function () {
            var gridAttachment = $("#gridAttachments").data("kendoGrid");
            if (gridAttachment) {
                data = { apk: $("#JobAPK").val() };
                gridAttachment.dataSource.read(data);
            }
        };

        function deleteAttachment_Click(e) {
            e.preventDefault();

            var tr = $(e.target).closest("tr"); //get the row for deletion
            var data = this.dataItem(tr); //get the row data so it can be referred later
            
            attachmentConfirmWindow.content(attachmentConfirmTemplate(data)); //send the row data object to the template and render it
            attachmentConfirmWindow.open().center();

            $("#yesButton").click(function () {
                $.ajax({
                    type: 'POST',
                    url: $('#removeAttachmentUrl').val(),
                    data: JSON.stringify(data),
                    contentType: 'application/json; charset=utf-8',
                    success: function () {
                        jobs.viewer.refreshAttachmentGrid();
                        attachmentConfirmWindow.close();
                    }
                });
            });

            $("#noButton").click(function () {
                window.close();
            });
        }

        this.initAttachmentGrid = function () {
            $("#gridAttachments").kendoGrid({
                toolbar: ['excel'],
                excel: 'attachments.xlsx',
                dataSource: {
                    type: "json",
                    transport: {
                        read: $('#gridAttachmentsUrl').val() + '?apk=' + $('#JobAPK').val(),
                    },
                    pageSize: 20
                },
                height: 500,
                groupable: true,
                resizable: true,
                selectable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: [
                    {
                        field: "APK",
                        title: "",
                        hidden: true,
                        width: 0
                    }, {
                        field: "AttachmentFileName",
                        title: "Tên tập tin",
                        width: 200,
                        template: "<a href='/Upload/Download/#: data.APK #' target='_blank'>#: data.AttachmentFileName #</a>"
                    }, {
                        field: "AttachmentFileSize",
                        title: "Kích thước",
                        width: 100
                    }, {
                        field: "AttachmentFileExtension",
                        title: "Phần mở rộng",
                        width: 100
                    }, {
                        field: "AttachmentFileType",
                        title: "Loại tập tin",
                        width: 100
                    }, {
                        field: "AttachmentOwner",
                        title: "Người đăng",
                        width: 100
                    }, {
                        field: "AttachmentComment",
                        title: "Mô tả"
                    }, {
                        field: "CreatedDate",
                        title: "Ngày tạo",
                        type: "date",
                        width: 120,
                        template: '#= kendo.toString(data.CreatedDate, "dd/MM/yyyy") #'
                    }, {
                        field: "CreatedUserID",
                        title: "Người tạo",
                        width: 120
                    }, {
                        field: "LastModifyDate",
                        title: "Ngày cập nhật",
                        type: "date",
                        width: 120,
                        template: '#= kendo.toString(data.LastModifyDate, "dd/MM/yyyy") #'
                    }, {
                        field: "LastModifyUserID",
                        title: "Người cập nhật",
                        width: 120
                    }, {
                        command: [
                            {
                                text: 'Sửa',
                                imageClass: 'k-icon k-i-pencil ob-icon-only',
                                title: 'Sửa',
                                click: updateAttachment_Click
                            }, {
                                text: 'Xóa',
                                imageClass: 'k-icon k-i-close ob-icon-only',
                                title: 'Xóa',
                                click: deleteAttachment_Click
                            }
                        ],
                        width: 160
                    }
                ]
            });
        };

        this.initAttachmentDeleteConfirmDialog = function () {
            attachmentConfirmTemplate = kendo.template($("#attachmentConfirmTemplate").html());
            attachmentConfirmWindow = $("#confirmDialog").kendoWindow({
                title: "Thông báo",
                visible: false,
                modal: true,
                width: "400px",
                height: "100px",
            }).data("kendoWindow");
        };
    };
}