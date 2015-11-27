(function($) {
    $.validator.setDefaults({
        highlight: function(t) {
            $(t).closest(".form-group").addClass("has-error");
        },
        unhighlight: function(t) {
            $(t).closest(".form-group").removeClass("has-error");
        },
        errorElement: "span",
        errorClass: "help-block help-block-validation-error",
        errorPlacement: function(t, i) {
            $(i).closest(".form-group").append(t);
        }
    });
})(jQuery)