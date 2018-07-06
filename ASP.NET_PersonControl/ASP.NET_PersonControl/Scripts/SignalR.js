$.connection.hub.start()
    .done(function () {
        console.log("connection.hub.start()")
            $.connection.hubSignalR.server.sendMessage(chatId, chatMessage);
    })
    .fail(function () { alert("ItFail connection.hub.start()") });

$.connection.hubSignalR.client.sendMessage = function (chatId, message) {
    $("#messages").append(" <tr><td> <img src=\"" + $("#img_src").val() + "\"  alt=\"Avatar\" style=\"width: 65px;height :65px;border-radius: 50%;\"/> </td> <td style=\"text-align: center;vertical-align: middle; \">" + chatId + "</td>  <td style=\"text-align: center;vertical-align: middle; \">" + message + "</td></tr>");
}
