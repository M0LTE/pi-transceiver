"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/uiHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("SetSMeter", function (value) {
    console.log(value);
    document.getElementById("smeter").innerText = value;
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