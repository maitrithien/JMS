$(document).ready(function () {
    $('#chkAll').bind('click', common.checkAll);
});

var users = new function () {
    this.user = new function () {
        var userConfirmTemplate = null;
        var userConfirmWindow = null;
        var userMessageTemplate = null;
        var userMessageWindow = null;

        this.refreshUserGrid = function () {
            var grid = $("#gridUsers").data("kendoGrid");
            if (grid) {
                data = { };
                grid.dataSource.read(data);
            }
        };

        function checkDeleteUser(data, func) {
            $.ajax({
                type: 'POST',
                url: $('#checkDeleteUserUrl').val(),
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                success: function (dataResult) {
                    if (!dataResult.result) {
                        userConfirmWindow.close();

                        userMessageWindow.content(userMessageTemplate(dataResult)); //send the row data object to the template and render it
                        userMessageWindow.open().center();

                        $("#okButton").click(function () {
                            userMessageWindow.close();
                        });
                    }
                    else {
                        $.ajax({
                            type: 'POST',
                            url: $('#removeUserUrl').val(),
                            data: JSON.stringify(data),
                            contentType: 'application/json; charset=utf-8',
                            success: function () {
                                var grid = $('#grid').data('kendoGrid');
                                if (grid) {
                                    var data = common.getFormDataTypeJson("frm-main-filter");
                                    data['IsFilter'] = 1;

                                    grid.dataSource.read(data);
                                }
                                userConfirmWindow.close();
                            }
                        });
                    }
                }
            });
        }

        function deleteUser_Click(e) {
            e.preventDefault();

            var tr = $(e.target).closest("tr"); //get the row for deletion
            var data = this.dataItem(tr); //get the row data so it can be referred later

            userConfirmWindow.content(userConfirmTemplate(data)); //send the row data object to the template and render it
            userConfirmWindow.open().center();

            $("#yesButton").click(function () {
                checkDeleteUser(data);
            });

            $("#noButton").click(function () {
                userConfirmWindow.close();
            });
        }

        this.initUserConfirmDialog = function () {
            userConfirmTemplate = kendo.template($("#userConfirmTemplate").html());
            userConfirmWindow = $("#userConfirmDialog").kendoWindow({
                title: "Thông báo",
                visible: false,
                modal: true,
                width: "400px",
                height: "100px",
            }).data("kendoWindow");

            userMessageTemplate = kendo.template($("#userMessageTemplate").html());
            userMessageWindow = $("#userMessageDialog").kendoWindow({
                title: "Thông báo",
                visible: false,
                modal: true,
                width: "400px",
                height: "100px",
            }).data("kendoWindow");

        };

        this.initUserGrid = function () {
            $("#grid").kendoGrid({
                toolbar: ['excel'],
                excel: {
                    fileName: 'users.xlsx'
                },
                dataSource: {
                    type: "json",
                    transport: {
                        read: $('#gridUsersUrl').val(),
                        data: function () {
                            var data = {
                                DeadlineFilter: '26/04/2015',
                                IsFilter: 1,

                            }
                            return data;
                        }//gridSendData
                    },
                    pageSize: 20
                },
                height: 500,
                groupable: {
                    messages: {
                        empty: "Kéo một tiêu đề cột và thả nó vào đây để nhóm theo cột đó"
                    }
                },
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
                        field: "UserID",
                        title: "Mã JOBS",
                        width: 120,
                        template: "<a href='/Users/UserView/#: data.APK #' target='_blank' title='Xem chi tiết JOBS #: data.UserID #'>#: data.UserID #</a>"
                    }, {
                        field: "UserName",
                        title: "Tên JOBS",
                        width: 200
                    }, {
                        command: [
                            {
                                text: 'Xóa',
                                imageClass: 'k-icon k-i-close ob-icon-only',
                                title: 'Xóa',
                                click: deleteUser_Click
                            }
                        ],
                        width: 80
                    }
                ]
            });
        };

        this.closeUserDialog = function () {
            var win = $("#userDialog").data("kendoWindow");
            if (win) {
                win.close();
            }
        };
    };
}