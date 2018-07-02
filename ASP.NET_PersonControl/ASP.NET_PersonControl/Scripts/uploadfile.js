$(document).on("click", "#btnid", function (event) {
    event.preventDefault();
    var fileOptions = {
        success: res,
        dataType: "json"
    }
    //$("#formid").ajaxSubmit(fileOptions);
    var action = $("#btnid").attr("Upload");
    $.ajax({
        type: "POST",
        url: action,
        dataType: "json",
        data: fileOptions,
        contentType: "application/json; multipart/form-data",
        success: function (data) {
            //perform the desired operations here eg. change labels and etc by getting the values from data object.
        },
        error: function () {
            console.log('error');
        }
    });
});