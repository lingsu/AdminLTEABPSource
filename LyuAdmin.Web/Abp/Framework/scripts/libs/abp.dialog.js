var app = app || {};
(function ($) {
    var loadScripts = [];
    app.modals = app.modals || {};
    app.ModalManager = function () {
        function init(t) {
            var r = t + "Container",
            u = "#" + r,
            i = $(u);
            i.length && i.remove();
        }
        function view(id) {
            init(id);
            var divId = id + "Container";
            return $('<div id="' + divId + '"><\/div>').append('<div id="' + id + '" class="modal fade" tabindex="-1" role="modal" aria-hidden="true">  <div class="modal-dialog">    <div class="modal-content"><\/div>  <\/div><\/div>').appendTo("body")
        }
        var newId = function (n) {
            n.modalId || (n.modalId = "Modal_" + Math.floor(Math.random() * 1e6) + (new Date).getTime());
        };
        return function (userOption) {
            var c, close;
            newId(userOption);
            var elc = null,
            modalId = userOption.modalId,
            p = "#" + modalId,
            modal = null,
            l = null,
            model = null;

            function save() {
                modal && modal.save && modal.save();
            }
            function buttonBusy(n) {
                elc && elc.find(".modal-footer button").button('toggle');
            }

            function viewShow() {
                elc = $(p);
                elc.modal({
                    backdrop: "static"
                });
                elc.on("shown.bs.modal",
               function () {
                   elc.find("input:first").focus();
               });

                var t = app.modals[userOption.modalClass];
                t && (modal = new t, modal.init && modal.init(l, model));
                elc.find(".save-button").click(function () {
                    save();
                });
                elc.find(".modal-body").keydown(function (n) {
                    n.which == 13 && (n.preventDefault(), save())
                });

                elc.modal("show");
            }
            c = function(data) {
                model = data || {};
                view(modalId).find(".modal-content").load(userOption.viewUrl, model, function (i, r) {
                    if (r === "error") {
                        abp.message.warn('页面读取失败');
                        return;
                    }

                    userOption.scriptUrl && $.inArray(loadScripts, userOption.scriptUrl) < 0 ? $.getScript(userOption.scriptUrl).done(function () {
                        loadScripts.push(userOption.scriptUrl);
                        viewShow();
                    }).fail(function() {
                        abp.message.warn('Javascript加载错误');
                    }) : viewShow();
                });
            }
            close = function () {
                elc && elc.modal("hide");
            };
            return l = {
                open: c,
                reopen: function () {
                    c(s);
                },
                close: close,
                getModalId: function () {
                    return modalId;
                },
                getModal: function () {
                    return elc;
                },
                getArgs: function () {
                    return model;
                },
                setBusy: buttonBusy
            }
        }
    }()
})(jQuery);