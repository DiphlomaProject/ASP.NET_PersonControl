function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function setCookie(sectionName) {
    var lastState = getCookie(sectionName);
    if (lastState == "" || lastState == "off") {
        document.cookie = sectionName + "=on";
    }
    else {
        document.cookie = sectionName + "=off";
    }
}

function setState() {
    if (getCookie("Panel1") == "" || getCookie("Panel1") == "off") {
        document.getElementById("Panel1").className = "collapse";
    }
    else {
        document.getElementById("Panel1").className = "collapse in";
    }

    if (getCookie("Panel2") == "" || getCookie("Panel2") == "off") {
        document.getElementById("Panel2").className = "collapse";
    }
    else {
        document.getElementById("Panel2").className = "collapse in";
    }

    if (getCookie("Panel3") == "" || getCookie("Panel3") == "off") {
        document.getElementById("Panel3").className = "collapse";
    }
    else {
        document.getElementById("Panel3").className = "collapse in";
    }
}