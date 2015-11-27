var abp = abp || {};
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (prefix) {
        return this.slice(0, prefix.length) === prefix;
    };
}
(function ($) {

    var config = {
        'default': {
            "language": {
                "processing": "处理中...",
                "search": "搜索",
                "lengthMenu": "每页 _MENU_ 条记录",
                "zeroRecords": "没有匹配结果",
                "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
                "infoEmpty": "无记录",
                "infoFiltered": "(从 _MAX_ 条记录过滤)",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "上页",
                    "sNext": "下页",
                    "sLast": "末页"
                },
                "sInfoThousands": ",",
                "oAria": {
                    "sSortAscending": ": 以升序排列此列",
                    "sSortDescending": ": 以降序排列此列"
                }
            },

            //searching: true,
            ordering: false,
            //"bSort": false,
            select: {
                style: 'info'
            },
            "processing": true,
            "serverSide": true
        }
    };


    abp.Datatables = function () {
        function ajaxExt(ajaxUrl) {

            return function (data, callback, settings) {

                $.ajax({
                    url: ajaxUrl,
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'POST',
                    contentType: 'application/json',
                    success: function (json) {
                        callback(json.result);
                    }
                });
            }
        }
        function columnExt(columns) {
            var cols = columns;
            for (var i = 0; i < cols.length; i++) {
                var col = cols[i];
                if (!col.render) {
                    (function (column) {
                        column.render = function (data, type, full, meta) {
                            var t = typeof data;
                            if (t === 'boolean') {
                                if (data) {
                                    return '<span class="label label-success">是</span>';
                                } else {
                                    return '<span class="label label-default">否</span>';
                                }
                            }
                            if (data && column.data.endsWith('Time')) {
                                return moment(data).format('YYYY-MM-DD HH:mm:ss');
                            }
                            return data;
                        }
                    })(col);
                }
            }
        }

        return function (userOptions) {
            var self,selectClass;
            self = this,
            self.table = $(userOptions.table) ;
            var opts = $.extend({}, config['default'], userOptions);
            if (opts.columns) {
                columnExt(opts.columns);
            }

            if (userOptions.ajax) {
                var ajaxUrl = userOptions.ajax;
                opts.ajax = ajaxExt(ajaxUrl);
            }
            self.datatable = self.table.DataTable(opts);
            self.table.on('click', 'tbody > tr', function (e) {
                if ('TD' === e.target.nodeName) {
                    $(this).toggleClass(opts.select.style);
                }
            });

            //刷新
            $("#btnReload").on('click', function () {
                self.reloadList();
            });

            self.getSelectedIdList = function () {
                return self.datatable.rows('.' + opts.select.style).data().toArray();
            }
            self.getSelectedOneRowData = function () {
                var data = self.getSelectedIdList();
                return data.length ? data[0] : null;
            }
            self.reloadList = function() {
                self.datatable.ajax.reload(null, false);
            }
        }
    }();
})(jQuery)