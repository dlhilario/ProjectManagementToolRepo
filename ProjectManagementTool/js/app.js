var app = (function () {
    "use strict"
    $(document).ready(function () {
        init();
    })

    function init() {
        manageUser(); 
    }

    var manageUser = function () {
        var columnsName = [];
        $(".manageUserTable").DataTable({
            "ajax": {
                "url": "/Data/GetUsers",
                "dataSrc": "",
            },
            rowReorder: {
                selector: 'td:nth-child(2)' // this for dragging on the second column
            },
            responsive: true,
            columns: [
                {
                    "data": "ID", render: function (data, type, full, meta) {
                        columnsName = [];
                        var columnsData = meta.settings.aoColumns;
                        for (var i = 0; i < columnsData.length; i++) {
                            if (columnsData[i].data != undefined) {
                                columnsName.push({ key: columnsData[i].data, value: "" });
                            }
                        }
                        return data;
                    }
                },
                { "data": "UserName" },
                { "data": "FirstName" },
                { "data": "LastName" },
                { "data": "Email" },
                { "data": "Active" },
                {
                    "data": "CreatedDate", render: function (data, type, full, meta) {
                        if (data) {
                            var d = data.replace(/[\/Date(*)]/g, "");
                            var date = new Date(Number(d));

                            return moment(date).format("MM/DD/YYYY hh:mm:ss A");
                        }
                        return null;
                    }
                },
                {
                    title: "action",
                    render: function (data, type, full, meta) {

                        return "<a href='javascript:void(0);' data-id='" + full.ID + "' data-toggle='modal' data-target='#manage_user'><i class=\"fa fa-edit\"></i> Edit</a> | <a href='javascript:app.DoDelete(" + full.ID + ");'><i class=\"fa fa-trash\"></i> Delete </a> ";
                    }
                }

            ]
        })

    }

    var CreateUser = function () {
        $(".edit_user").dialog({
            title: "Edit Account",
            modal: true,
            //height: 400,
            width: 600,
            buttons: {
                EditUser: {
                    text: "Edit User",
                    icon: "ui-icon-heart",
                    className: "btn btn-default"
                },
                Cancel: {
                    text: "Cancel",
                    className: "btn btn-default",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            }
        })
    };

    var EditUser = function () {

    }

    $("#manage_user").on("show.bs.modal", function (e) {
        loadFormData(e.relatedTarget.dataset.id)
    });

    $("#btnSaveIt").on("click", function (e) {
        e.preventDefault();
        console.log("i am click");
        var $form = $("#manage_user_form");
        $form.validate({
            debug: true,
            rules: {
                firstname: {
                    required: true
                },
                lastname: {
                    required: true
                },
                password: {
                    required: true,
                    minlength: 5
                },
                confirmpassword: {
                    required: true,
                    equalTo: "#password",
                    minlength: 5
                }
            },
            messages: {
                firstname: "First name required",
                lastname: "Last name required",
                password: {
                    required: "Password required",
                    minlength: "Your password must be a least 5 charaters"
                },
                confirmpassword: {
                    required: "Confirm password required",
                    minlength: "Your password must be a least 5 charaters"
                }
            },

            submitHandler: function (form) {
                var formData = $(form).serialize();
                $.post( "/Data/EditUser", formData).done(function (data) {
                    form.submit();
                })

            }
        });
        $form.submit();


    });

    function loadFormData(userId) {
        $.get("/Data/GetUsers", { userId: userId }).done(function (data) {
            var $form = $("#manage_user_form");
            $("#user_id").remove();
            $("<input type='hidden' id='user_id' name='user_id' />").val(userId).appendTo($form);
            $form.find("#username").val(data.UserName);
            $form.find("#firstname").val(data.FirstName);
            $form.find("#lastname").val(data.LastName);
            $form.find("#emailaddress").val(data.Email);

        });
    }

    var service = {
        CreateUser: CreateUser
    }

    return service;
}());