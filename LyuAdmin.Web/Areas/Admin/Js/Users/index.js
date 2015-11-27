(function () {
    $(function () {
        var _userService = abp.services.app.user;

        var _createOrEditModal = new app.ModalManager({
            viewUrl: '/admin/user/Edit/',
            scriptUrl: abp.appPath + 'Areas/admin/js/Users/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserModal'
        });

        var columns = [
            {
                title: '操作',
                width: '9%',
                data: 'id',
                visible: _permissions.edit || _permissions.changePermissions || _permissions.delete,
                createdCell: function (cell, cellData, rowData, rowIndex, colIndex) {
                    var $span = $('<span></span>');
                    if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="编辑"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .on('click',function (e) {
                                _createOrEditModal.open({ id: cellData });
                            });
                    }

                    if (_permissions.changePermissions) {
                        $('<button class="btn btn-default btn-xs" title="权限"><i class="fa fa-list"></i></button>')
                            .appendTo($span)
                            .on('click', function () {
                                //_userPermissionsModal.open({ id: cellData });
                            });
                    }

                    if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="删除"><i class="fa fa-trash-o"></i></button>')
                            .appendTo($span)
                            .on('click', function () {
                                deleteUser(rowData);
                            });
                    }
                    $span.appendTo($(cell));
                },
                render: function (data, type, full, meta) {
                    return '';
                }
            },
            { data: 'userName', title: '用户名' },
            { data: 'name', title: '名字' },
            { data: 'surname', title: '姓氏' },
            {
                data: 'assignedRoleNames',
                title: '角色',
                render: function (data) {
                    var roleNames = '';
                    for (var j = 0; j < data.length; j++) {
                        if (roleNames.length) {
                            roleNames = roleNames + ', ';
                        }
                        roleNames = roleNames + data[j].name;
                    };
                    return roleNames;
                }
            },
            { data: 'emailAddress', title: '邮箱地址' },
            { data: 'isEmailConfirmed', title: '邮箱地址验认' },
            { data: 'isActive', title: '激活' },
            {data: 'lastLoginTime',title: '上次登录时间'},
            {data: 'creationTime',title: '创建时间'}
        ];

        var databales = new abp.Datatables({
            table: '#jqtable',
            columns: columns,
            ajax: abp.appPath + "api/services/app/user/GetUserQuery",
            "autoWidth": true
        });

        abp.event.on('app.createOrEditUserModalSaved', function () {
            databales.reloadList();
        });

        //新增
        $("#btnNew").click(function () {
            _createOrEditModal.open();
        });

        function deleteUser(user) {
            if (user.userName === abp.consts.userManagement.defaultAdminUserName) {
                abp.message.warn(abp.utils.formatString("不能删除{0}用户", app.consts.userManagement.defaultAdminUserName));
                return;
            }

            abp.message.confirm(
                abp.utils.formatString("您确认要删除{0}用户吗？", user.userName),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.deleteUser({
                            id: user.id
                        }).done(function () {
                            databales.reloadList();
                            abp.notify.success('删除成功');
                        });
                    }
                }
            );
        }

        ////删除
        //$("#btnDelete").on('click', function () {
        //    var row = abp.grid.getSelectedOneRowData();
        //    if (!row) return;
        //    abp.confirm("您确认要删除选中的行吗？", function (result) {
        //        if (!result) return; //取消
        //        var data = { id: row.id };
        //        abp.ajax({
        //            url: abp.appPath + 'api/services/<#= ModuleName #>/<#= EntityName #>/Delete<#= EntityName #>',
        //            data: data
        //        }).done(function (ret) {
        //            abp.success("删除成功");
        //            abp.grid.reloadList();
        //        });
        //    });
        //});
        ////批量删除
        $("#btnDelete").on('click', function () {
            var idList = databales.getSelectedIdList();
            if (idList.length === 0) return;
            abp.message.confirm(abp.utils.formatString("您确认要删除选中的{0}行吗？", idList.length), function (isConfirmed) {
                if (!isConfirmed) return; //取消
                abp.ajax({
                    url: '/api/services/user/User/BatchDeleteUser',
                    data: idList
                }).done(function (ret) {
                    abp.message.success("删除成功");
                    databales.reloadList();
                });
            });
        });
    });
})()