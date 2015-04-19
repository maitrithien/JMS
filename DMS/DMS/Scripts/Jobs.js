$(document).ready(function () {
    jobs.attachment.initAttachmentGrid();
    jobs.attachment.initAttachmentDeleteConfirmDialog();
    $('#chkAll').bind('click', common.checkAll);

    var kgroup = $('.k-grouping-header');
    if (kgroup) {
        $.each(kgroup, function (i, item) {
            $(item).text("Kéo một tiêu đề cột và thả nó vào đây để nhóm theo cột đó");
        });
    }
});

var jobs = new function () {
    this.attachment = new function () {
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
                        jobs.attachment.refreshAttachmentGrid();
                        attachmentConfirmWindow.close();
                    }
                });
            });

            $("#noButton").click(function () {
                attachmentConfirmWindow.close();
            });
        }

        this.initAttachmentGrid = function () {
            $("#gridAttachments").kendoGrid({
                toolbar: ['excel'],
                excel: {
                    fileName: 'attachments.xlsx'
                },
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
                        title: "Mô tả",
                        width: 200,
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

    this.job = new function () {
        var jobConfirmTemplate = null;
        var jobConfirmWindow = null;
        var jobMessageTemplate = null;
        var jobMessageWindow = null;

        this.refreshJobGrid = function () {
            var grid = $("#gridJobs").data("kendoGrid");
            if (grid) {
                data = { };
                grid.dataSource.read(data);
            }
        };

        function checkDeleteJob(data, func) {
            $.ajax({
                type: 'POST',
                url: $('#checkDeleteJobUrl').val(),
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (dataResult) {
                    if (!dataResult.result) {
                        jobConfirmWindow.close();

                        jobMessageWindow.content(jobMessageTemplate(dataResult)); //send the row data object to the template and render it
                        jobMessageWindow.open().center();

                        $("#okButton").click(function () {
                            jobMessageWindow.close();
                        });
                    }
                    else {
                        $.ajax({
                            type: 'POST',
                            url: $('#removeJobUrl').val(),
                            data: JSON.stringify(data),
                            contentType: 'application/json; charset=utf-8',
                            success: function () {
                                var grid = $('#grid').data('kendoGrid');
                                if (grid) {
                                    var data = common.getFormDataTypeJson("frm-main-filter");
                                    data['IsFilter'] = 1;

                                    grid.dataSource.read(data);
                                }
                                jobConfirmWindow.close();
                            }
                        });
                    }
                }
            });
        }

        function deleteJob_Click(e) {
            e.preventDefault();

            var tr = $(e.target).closest("tr"); //get the row for deletion
            var data = this.dataItem(tr); //get the row data so it can be referred later

            jobConfirmWindow.content(jobConfirmTemplate(data)); //send the row data object to the template and render it
            jobConfirmWindow.open().center();

            $("#yesButton").click(function () {
                checkDeleteJob(data);
            });

            $("#noButton").click(function () {
                jobConfirmWindow.close();
            });
        }

        this.initJobConfirmDialog = function () {
            jobConfirmTemplate = kendo.template($("#jobConfirmTemplate").html());
            jobConfirmWindow = $("#jobConfirmDialog").kendoWindow({
                title: "Thông báo",
                visible: false,
                modal: true,
                width: "400px",
                height: "100px",
            }).data("kendoWindow");

            jobMessageTemplate = kendo.template($("#jobMessageTemplate").html());
            jobMessageWindow = $("#jobMessageDialog").kendoWindow({
                title: "Thông báo",
                visible: false,
                modal: true,
                width: "400px",
                height: "100px",
            }).data("kendoWindow");

        };

        this.initJobGrid = function () {
            $("#grid").kendoGrid({
                toolbar: ['excel'],
                excel: {
                    fileName: 'jobs.xlsx'
                },
                dataSource: {
                    type: "json",
                    transport: {
                        read: $('#gridJobsUrl').val()
                    },
                    pageSize: 20
                },
                height: 500,
                groupable: true,
                resizable: true,
                //sortable: true,
                selectable: true,
                //filterable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: [
                    {
                        field: "APK",
                        title: "",
                        width: 0,
                        hidden: true
                    }, {
                        field: "JobID",
                        title: "Mã hồ sơ",
                        width: 120,
                        template: "<a href='/Jobs/JobView/#: data.APK #' target='_blank' title='Xem chi tiết hồ sơ #: data.JobID #'>#: data.JobID #</a>"
                    }, {
                        field: "JobName",
                        title: "Tên hồ sơ",
                        width: 200
                    }, {
                        field: "StatusName",
                        title: "Tình trạng",
                        width: 100
                    }, {
                        field: "CreatedUserName",
                        title: "Người tạo",
                        width: 150
                    }, {
                        field: "PosterName",
                        title: "Người lập",
                        width: 150
                    }, {
                        field: "DepartmentName",
                        title: "Phòng nhận hồ sơ",
                        width: 150
                    }, {
                        field: "RecipientName",
                        title: "Người thực hiện",
                        width: 150
                    }, {
                        field: "ConfirmerName",
                        title: "Người duyệt",
                        width: 150
                    }, {
                        field: "CreatedDate",
                        title: "Ngày tạo",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "Deadline",
                        title: "Ngày hết hạn",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "RateName",
                        title: "Đánh giá",
                        width: 100
                    }, {
                        field: "PriorityName",
                        title: "Độ ưu tiên",
                        width: 100
                    }, {
                        field: "ComplexName",
                        title: "Độ phức tạp",
                        width: 100
                    }, {
                        command: [
                            {
                                text: 'Xóa',
                                imageClass: 'k-icon k-i-close ob-icon-only',
                                title: 'Xóa',
                                click: deleteJob_Click
                            }
                        ],
                        width: 80
                    }
                ]
            });
        };

        this.initJobGridOther = function () {
            $("#grid").kendoGrid({
                toolbar: ['excel'],
                excel: {
                    fileName: 'jobs.xlsx'
                },
                dataSource: {
                    type: "json",
                    transport: {
                        read: $('#gridJobsUrl').val()
                    },
                    pageSize: 20
                },
                height: 500,
                groupable: true,
                resizable: true,
                //sortable: true,
                selectable: true,
                //filterable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: [
                    {
                        field: "APK",
                        title: "",
                        width: 0,
                        hidden: true
                    }, {
                        field: "JobID",
                        title: "Mã hồ sơ",
                        width: 120,
                        template: "<a href='/Jobs/JobView/#: data.APK #' target='_blank' title='Xem chi tiết hồ sơ #: data.JobID #'>#: data.JobID #</a>"
                    }, {
                        field: "JobName",
                        title: "Tên hồ sơ",
                        width: 200
                    }, {
                        field: "StatusName",
                        title: "Tình trạng",
                        width: 100
                    }, {
                        field: "CreatedUserName",
                        title: "Người tạo",
                        width: 150
                    }, {
                        field: "PosterName",
                        title: "Người gửi",
                        width: 150
                    }, {
                        field: "DepartmentName",
                        title: "Phòng nhận hồ sơ",
                        width: 150
                    }, {
                        field: "RecipientName",
                        title: "Người thực hiện",
                        width: 150
                    }, {
                        field: "ConfirmerName",
                        title: "Người duyệt",
                        width: 150
                    }, {
                        field: "CreatedDate",
                        title: "Ngày tạo",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "Deadline",
                        title: "Ngày hết hạn",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "RateName",
                        title: "Đánh giá",
                        width: 100
                    }, {
                        field: "PriorityName",
                        title: "Độ ưu tiên",
                        width: 100
                    }, {
                        field: "ComplexName",
                        title: "Độ phức tạp",
                        width: 100
                    }
                ]
            });
        };

        this.initJobGridOver = function () {
            $("#grid").kendoGrid({
                toolbar: ['excel'],
                excel: {
                    fileName: 'jobs.xlsx'
                },
                dataSource: {
                    type: "json",
                    transport: {
                        read: $('#gridJobsUrl').val()
                    },
                    pageSize: 20
                },
                height: 500,
                groupable: true,
                resizable: true,
                //sortable: true,
                selectable: true,
                //filterable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: [
                    {
                        field: "APK",
                        title: "",
                        width: 0,
                        hidden: true
                    }, {
                        field: "JobID",
                        title: "Mã hồ sơ",
                        width: 120,
                        template: "<a href='/Jobs/JobView/#: data.APK #' target='_blank' title='Xem chi tiết hồ sơ #: data.JobID #'>#: data.JobID #</a>"
                    }, {
                        field: "JobName",
                        title: "Tên hồ sơ",
                        width: 200
                    },  {
                        field: "StatusName",
                        title: "Tình trạng",
                        width: 100
                    }, {
                        field: "OverDeadlineNumber",
                        title: "Số ngày quá hạn",
                        width: 100
                    }, {
                        field: "CreatedUserName",
                        title: "Người tạo",
                        width: 150
                    }, {
                        field: "PosterName",
                        title: "Người gửi",
                        width: 150
                    }, {
                        field: "DepartmentName",
                        title: "Phòng nhận hồ sơ",
                        width: 150
                    }, {
                        field: "RecipientName",
                        title: "Người thực hiện",
                        width: 150
                    }, {
                        field: "ConfirmerName",
                        title: "Người duyệt",
                        width: 150
                    }, {
                        field: "CreatedDate",
                        title: "Ngày tạo",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "Deadline",
                        title: "Ngày hết hạn",
                        width: 120,
                        type: "date",
                        template: '#= kendo.toString(data.Deadline, "dd/MM/yyyy") #'
                    }, {
                        field: "RateName",
                        title: "Đánh giá",
                        width: 100
                    }, {
                        field: "PriorityName",
                        title: "Độ ưu tiên",
                        width: 100
                    }, {
                        field: "ComplexName",
                        title: "Độ phức tạp",
                        width: 100
                    }
                ]
            });
        };

        this.closeJobDialog = function () {
            var win = $("#jobDialog").data("kendoWindow");
            if (win) {
                win.close();
            }
        };
    };
}