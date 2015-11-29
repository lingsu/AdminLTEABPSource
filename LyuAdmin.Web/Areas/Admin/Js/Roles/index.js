
var listColumns = [
					listCheckboxColumn,
					{ "name": "id", "data": "id", title: "ID", "sortable": false, "visible": false },
					{ "name": "tenant", "data": "tenant", title: "Tenant" },
					{ "name": "displayName", "data": "displayName", title: "string" },
					{ "name": "isStatic", "data": "isStatic", title: "bool", width: "60px", className: "center", render: abp.grid.renderBool },
					{ "name": "isDefault", "data": "isDefault", title: "bool", width: "60px", className: "center", render: abp.grid.renderBool },
					{ "name": "permissions", "data": "permissions", title: "ICollection<RolePermissionSetting>" },
					{ "name": "deleterUser", "data": "deleterUser", title: "User" },
					{ "name": "creatorUser", "data": "creatorUser", title: "User" },
					{ "name": "lastModifierUser", "data": "lastModifierUser", title: "User" },
					{ "name": "tenantId", "data": "tenantId", title: "Nullable<int>" },
					{ "name": "name", "data": "name", title: "string" },
					{ "name": "isDeleted", "data": "isDeleted", title: "bool", width: "60px", className: "center", render: abp.grid.renderBool },
					{ "name": "deleterUserId", "data": "deleterUserId", title: "Nullable<long>" },
					{ "name": "deletionTime", "data": "deletionTime", title: "Nullable<DateTime>" },
					{ "name": "lastModificationTime", "data": "lastModificationTime", title: "Nullable<DateTime>" },
					{ "name": "lastModifierUserId", "data": "lastModifierUserId", title: "Nullable<long>" },
					{ "name": "creationTime", "data": "creationTime", title: "DateTime", width: "180px", render: abp.grid.renderDateTime },
					{ "name": "creatorUserId", "data": "creatorUserId", title: "Nullable<long>" },
				];

$(function () {
    abp.grid.init([
    	{
            order: [[abp.grid.getColumnIndex('creationTime'), 'desc']],
    	    table: "#mytable",
    	    ajax: abp.grid.ajaxLoadEx({
    	        "url": abp.appPath + "api/services/Roles/Role/GetRoleQuery",
    	        "initData": "#listCacheJson"
    	    }),
    	    columns: listColumns,
    	},
		{ switchs: false } //{ detail: { exclude: [0, 1] } }
    ]);

    //新增
    $("#btnNew").click(function () {
        abp.dialog({
            width: "1000px",
            height: "700px",
            title: "新增FunctionName",
            href: abp.appPath + 'Roles/Role/Edit',
            callback: abp.grid.reloadList
        });
    });

    //编辑
    $("#btnEdit").on('click', function () {
        var row = abp.grid.getSelectedOneRowData();
        if (!row) return;
        abp.dialog({
            width: "800px",
            height: "500px",
            title: "编辑FunctionName",
            href: abp.appPath + 'Roles/Role/Edit/' + row.id,
            callback: abp.grid.reloadList
        });
    });

    //批量删除
    $("#btnDelete").on('click', function () {
        var idList = abp.grid.getSelectedIdList();
        if (idList.length == 0) return;
        abp.confirm(abp.utils.formatString("您确认要删除选中的{0}行吗？", idList.length), function (result) {
            if (!result) return; //取消
            abp.ajax({
                url: abp.appPath + 'api/services/Roles/Role/BatchDeleteRole',
                data: idList
            }).done(function (ret) {
                abp.success("删除成功");
                abp.grid.reloadList();
            });
        });
    });
})


