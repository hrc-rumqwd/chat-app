function getCurrentUserInfo() {
    $.ajax({
        url: '/api/users/current',
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            localStorage.setItem('currentUser', JSON.stringify(data.data))
        },
        error: function (xhr) {
            if (xhr.status == 401)
                console.log("Unauthorized");
        }
    })
}

function setMessageToBoard(senderName, content) {
    const li = document.createElement("li");
    li.textContent = `${senderName}: ${content}`;
    document.getElementById("messageBox").appendChild(li);
}

function getChatHistory() {
    $.ajax({
        url: '/api/chat?pageIndex=1&pageSize=10',
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            data.data.forEach(message =>
                setMessageToBoard(message.senderName, message.content)
            );
        },
        error: function (data) {
            if (data.status == 400)
                console.log(data);
        }
    })
}

getCurrentUserInfo();
getChatHistory();

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

connection.on("ReceiveMessage", (result) => {
    setMessageToBoard(result.data.senderName, result.data.content);
});

connection.on("OnSendMessageError", (data) => {
    // Handle the error (e.g., display an error message to the user)
    console.error(data.data.error);
});

async function sendMessage(e) {
    debugger;
    const userInfo = JSON.parse(localStorage.getItem('currentUser'));
    const message = document.getElementById("messageInput").value;

    await connection.invoke("SendMessage", message, userInfo.id);
}

connection.start();


