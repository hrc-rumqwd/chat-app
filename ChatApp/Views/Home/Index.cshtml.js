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

function getChatHistory() {
    $.ajax({
        url: '/api/chat?pageIndex=1&pageSize=10',
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            data.data.forEach(message =>
                addMessageToBoard(message.senderName, message.content, message.sentAt)
            );
        },
        error: function (data) {
            if (data.status == 400)
                console.log(data);
        }
    })
}

function createChatItem(thumbnail, senderName, content, sentAt) {
    const li = $('<li>').addClass('d-flex justify-content-between mb-4');
    let sentAtTitle = "ago";

    // Calculate sent at time with current time
    const current = new Date();
    const sentAtDate = new Date(Date.parse(sentAt));

    // this calculate the time difference in miliseconds
    const duration = current - sentAtDate;
    const timeDifferenceMins = Math.floor(duration / 60000);
    const timeDifferenceHours = Math.floor(duration / 3600000);
    const timeDifferenceDays = Math.floor(duration / 86400000);

    if (timeDifferenceMins < 60) {
        sentAtTitle = `${timeDifferenceMins < 60000 ? "1 minute" : `${timeDifferenceMins} minutes`} ${sentAtTitle}`;
    }
    else if (timeDifferenceHours < 24) {
        sentAtTitle = `${timeDifferenceHours <= 1 ? "1 hour" : `${timeDifferenceHours} hours`} ${sentAtTitle}`;
    }
    else if (timeDifferenceDays < 30) {
        sentAtTitle = `${timeDifferenceHours <= 1 ? "1 day" : `${timeDifferenceHours} days`} ${sentAtTitle}`;
    }

    li.html(`
        <img src="${thumbnail}" alt="avatar"
            class="rounded-circle d-flex align-self-start me-3 shadow-1-strong" width="60">
            <div class="card">
                <div class="card-header d-flex justify-content-between p-3">
                    <p class="fw-bold mb-0">${senderName}</p>
                    <p class="text-muted small mb-0"><i class="far fa-clock"></i> ${sentAtTitle}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">${content}</p>
                </div>
            </div>
    `);

    return li;
}


function addMessageToBoard(senderName, content, sentAt) {
    const item = createChatItem("/images/avatar.png", senderName, content, sentAt);
    $("#messageBoard").append(item);
}

getCurrentUserInfo();
getChatHistory();

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

connection.on("ReceiveMessage", (result) => {
    addMessageToBoard(result.data.senderName, result.data.content, result.data.sentAt);
});

connection.on("OnSendMessageError", (data) => {
    // Handle the error (e.g., display an error message to the user)
    console.error(data.data.error);
});

async function sendMessage(e) {
    const userInfo = JSON.parse(localStorage.getItem('currentUser'));
    const message = document.getElementById("messageInput").value;

    await connection.invoke("SendMessage", message, userInfo.id);

    // Clear the input field after sending the message
    document.getElementById("messageInput").value = "";
}

connection.start();



