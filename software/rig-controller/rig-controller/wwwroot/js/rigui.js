"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/uiHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("SetSMeter", function (value) {
    console.log(value);
    document.getElementById("smeter").innerText = value;
});

connection.on("SetFrequency", function (digit1, digit2, digit3, digit4, digit5, digit6, digit7) {
    //console.log(value);
    document.getElementById("digit1").innerText = digit1;
    document.getElementById("digit2").innerText = digit2;
    document.getElementById("digit3").innerText = digit3;
    document.getElementById("digit4").innerText = digit4;
    document.getElementById("digit5").innerText = digit5;
    document.getElementById("digit6").innerText = digit6;
    document.getElementById("digit7").innerText = digit7;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var digit1 = 0;

document.getElementById("sendButton").addEventListener("click", function (event) {
    digit1++;
    connection.invoke("SetFrequencyDigit", 1, digit1).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});