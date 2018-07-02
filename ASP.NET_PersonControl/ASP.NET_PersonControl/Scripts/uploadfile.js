$(document).on("click", "#btnid", function (event) {
    event.preventDefault();
    var fileOptions = {
        success: res,
        dataType: "json"
    }
    $("#formid").ajaxSubmit(fileOptions);
});