"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let received = `<div class="incoming_msg">
                               <div class="incoming_msg_img">
                                 <img src="" alt="${user}">
                                               </div>
                                                <div class="received_msg">
                                                    <div class="received_withd_msg">
                                                        <p>
                                                                                   ${msg}
                                                        </p>
                                                        <span class="time_date">${new Date()}</span>
                                                    </div>
                                                </div>
                                            </div>`

    $("#msg_history").append(received);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var sender = document.getElementById("senderId").value;
    var receiver = document.getElementById("receiverId").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("JoinGroup", receiver).catch(function (err) {
        return console.error(err.toString());
    });

    var message = document.getElementById("messageInput").value;

    let send = `<div class="outgoing_msg">
                                  <div class="sent_msg">
                                      <p>
                                                  ${message}
                                      </p>
                                      <span class="time_date"> ${new Date()}</span>
                                  </div>
                              </div>`

    $("#msg_history").append(send);

    connection.invoke("SendMessageToGroup", sender, receiver, message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});