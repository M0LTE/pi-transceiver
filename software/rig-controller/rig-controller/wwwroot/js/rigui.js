"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/uiHub").build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

connection.on("SetSMeter", function (value) {
    console.log(value);
    document.getElementById("smeter").innerText = value;
});

connection.on("SetFrequency", function (digit1, digit2, digit3, digit4, digit5, digit6, digit7) {
    console.log("SetFrequency" + digit1 + digit2 + digit3 + digit4 + "." + digit5 + digit6 + digit7)
    document.getElementById("digit1").innerText = digit1;
    document.getElementById("digit2").innerText = digit2;
    document.getElementById("digit3").innerText = digit3;
    document.getElementById("digit4").innerText = digit4;
    document.getElementById("digit5").innerText = digit5;
    document.getElementById("digit6").innerText = digit6;
    document.getElementById("digit7").innerText = digit7;
});

connection.on("AddLogLine", function (msg) {
    var li = document.createElement("li");
    var list = document.getElementById("log");
    list.insertBefore(li, list.firstChild);
    li.textContent = msg;
});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
    connection.invoke("TriggerFrequencyUpdate").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("digit1up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 1, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit2up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 2, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit3up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 3, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit4up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 4, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit5up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 5, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit6up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 6, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit7up").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 7, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit1down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 1, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit2down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 2, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit3down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 3, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit4down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 4, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit5down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 5, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit6down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 6, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("digit7down").addEventListener("click", function (event) {
    connection.invoke("SetFrequencyDigit", 7, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

