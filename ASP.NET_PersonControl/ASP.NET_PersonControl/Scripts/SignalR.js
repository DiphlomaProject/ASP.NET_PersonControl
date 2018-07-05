$.connection.hub.start()
    .done(function () {
        console.log("connection.hub.start()")
            $.connection.hubSignalR.server.sendMessage(chatId, chatMessage);
    })
    .fail(function () { alert("ItFail connection.hub.start()") });

$.connection.hubSignalR.client.sendMessage = function (chatId, message) {
    $("#messages").append(chatId + " : " + message + "<br/>");
}