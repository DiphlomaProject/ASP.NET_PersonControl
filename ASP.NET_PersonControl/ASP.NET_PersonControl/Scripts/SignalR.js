$.connection.hub.start()
    .done(function () {
        console.log("ItWork!")
        $.connection.hubSignalR.server.sendMessage("chatId","Message!");
    })
    .fail(function () { alert("ItFail!") });

$.connection.hubSignalR.client.sendMessage = function (chatId, message) {
    $("#messages").append(chatId + " : " + message + "<br/>");
}