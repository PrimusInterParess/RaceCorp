"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {

    console.log('sender message' + message['senderId']);
    console.log('receiver message' + message['receiverId']);
    console.log('receiver divEl' + document.getElementById("receiverId").value);
    console.log('sender divEl' + document.getElementById("senderId").value);



    if (message['senderId'] !== message['receiverId'] && message['senderId'] !== document.getElementById("senderId").value && message['senderId'] === document.getElementById("receiverId").value) {
        let received = `<div class="incoming_msg">
                                       <div class="incoming_msg_img">
                                       <img src="${message['senderProfilePicurePath']}" alt="OleMale">
                                             </div>
                                              <div class="received_msg">
                                                  <div class="received_withd_msg">
                                                      <p>
                                                                                 ${message['content']}
                                                      </p>
                                                      <span class="time_date">${message['createdOn']}</span>
                                                  </div>
                                              </div>
                                          </div>`

        $("#msg_history").append(received);

        var objDiv = document.getElementById("msg_history");
        objDiv.scrollTop = objDiv.scrollHeight;
    }

});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    event.preventDefault();

    var receiver = document.getElementById("receiverId").value;
    var message = document.getElementById("messageInput").value;

    if (message !== '' && message!== '   ') {
        var message = document.getElementById("messageInput").value;

        let send = `<div class="outgoing_msg">
                      <div class="sent_msg">
                          <p>
                                      ${message}
                          </p>
                                  <span class="time_date"> </span>
                      </div>
                  </div>`

        $("#msg_history").append(send);

        var objDiv = document.getElementById("msg_history");
        objDiv.scrollTop = objDiv.scrollHeight;

        document.getElementById("messageInput").value = '';

        connection.invoke("SendMessageToGroup", receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    }


});

var eventElements = document.querySelectorAll(".custom").forEach(e => e.addEventListener("click", (evnt) => {
    evnt.preventDefault();

    let el = evnt.target.parentElement;

    let userId = el.getAttribute('userId');
    let interlocutorId = el.getAttribute('interlocutorId');

    document.getElementById("senderId").value = userId;
    document.getElementById("receiverId").value = interlocutorId;

    // Create and Send the request
    var fetch_status;
    fetch(`/api/message/messages?authorId=${userId}&interlocutorId=${interlocutorId}`, {
        method: "GET",
        headers: {
            "Content-type": "application/json;charset=UTF-8"
        }
    })
        .then(function (response) {
            // Save the response status in a variable to use later.
            fetch_status = response.status;
            // Handle success
            // eg. Convert the response to JSON and return
            return response.json();
        })
        .then(function (json) {
            // Check if the response were success
            if (fetch_status == 200) {
                // Use the converted JSON

                document.getElementById("sendButton").disabled = false;

                connection.invoke("JoinGroup", interlocutorId).catch(function (err) {
                    return console.error(err.toString());
                });

                ProcessMessages(json['messages']);

                var objDiv = document.getElementById("msg_history");
                objDiv.scrollTop = objDiv.scrollHeight;

                function ProcessMessages(messages) {

                    document.querySelector('.msg_history').remove();

                    var messageHistoryEl = ` <div class="msg_history" id="msg_history">
                                             </div>`;

                    $("#mesgs").prepend(messageHistoryEl);


                    for (const message of messages) {


                        let senderId = message['senderId'];


                        if (senderId === document.getElementById("senderId").value) {
                            GenerateSendMessageElement(message);
                        }
                        else {
                            GenerateReceivedMessageElement(message);
                        }

                        function GenerateSendMessageElement(message) {

                            let content = message['content'];
                            let createdOn = message['createdOn'];


                            let sendMessageEl = `<div class="outgoing_msg">
                                                       <div class="sent_msg">
                                                       <p>
                                                           ${content}
                                                       </p>
                                                       <span class="time_date"> ${createdOn}</span>
                                                   </div>
                                               </div>`

                            $("#msg_history").append(sendMessageEl);
                        }

                        function GenerateReceivedMessageElement(message) {


                            let senderProfilePicturePath = message['senderProfilePicturePath'];

                            let content = message['content'];
                            let createdOn = message['createdOn'];

                            let receivedMessageEl = `<div class="incoming_msg"> <div class="incoming_msg_img">
                                                           <img src="${senderProfilePicturePath}" alt="sunil">
                                                           </div>
                                                   <div class="received_msg">
                                                       <div class="received_withd_msg">
                                                           <p>
                                                                      ${content}
                                                           </p>
                                                           <span class="time_date">${createdOn}</span>
                                                       </div>
                                                      </div>
                                                  </div>`

                            $("#msg_history").append(receivedMessageEl);
                        }
                    }

                }
            }

        })
        .catch(function (error) {
            // Catch errors
            console.log(error);
        });
}));
