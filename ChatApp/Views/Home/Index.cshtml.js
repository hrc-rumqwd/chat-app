let isLoading = false;
let currentUserPage = 1;
let currentMessagePage = 1;
let isFetchingOlderMessages = false;

const userList = $("ul#userList");
const trigger = $("div#loadingTrigger");
const messageBoard = $("ul#messageBoard");
const topSentiel = $("div#topSentinel");

const observer = new IntersectionObserver((entries) => {
    // If the sentinel is visible, load more!
    if (entries[0].isIntersecting) {
        LoadUsers();
    }
}, { threshold: 1.0 });
observer.observe(trigger[0]);

const topObserver = new IntersectionObserver((entries) => {
    if (entries[0].isIntersecting && !isFetchingOlderMessages) {
        getChatHistory();
    }
}, { threshold: 1.0 });
topObserver.observe(topSentiel[0]);

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
    isFetchingOlderMessages = true;

    // Capture the height before adding new messages
    const previousHeight = messageBoard.prop("scrollHeight");

    // Fetching older messages
    $.ajax({
        url: `/api/chat?pageIndex=${currentMessagePage}&pageSize=10`,
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            if (data.data.length > 0) {
                data.data
                    .forEach(message => {
                        // Prepend messages (add them AFTER the sentinel but BEFORE existing messages)
                        const item = createChatItem("/images/avatar.png", message.senderName, message.content, message.sentAt);
                        // Insert after the sentinel
                        topSentiel.after(item);
                    });

                // Adjust scroll position so the user doesn't lose their place
                // New Height - Old Height = The exact distance to maintain the view
                messageBoard.prop("scrollTop", messageBoard.prop("scrollHeight") - previousHeight);
                currentMessagePage += 1;
            }
            else {
                topObserver.unobserve(topSentiel[0]);
            }

            isFetchingOlderMessages = false;
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
        sentAtTitle = `${timeDifferenceDays <= 1 ? "1 day" : `${timeDifferenceDays} days`} ${sentAtTitle}`;
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

function createUserItem(userId, thumbnail, name, lastMessage, isOnline) {
    const li = $("<li>").addClass("p-2 border-bottom")
        .attr("id", userId);
    const userDisplayItem = $("<div>").addClass("d-flex justify-content-between");
    li.append(userDisplayItem);

    userDisplayItem.html(`
        <div class="pt-1 d-flex flex-row">
                <img src="${thumbnail}" alt="avatar"
                class="rounded-circle d-flex align-self-center me-3 shadow-1-strong" width="60" />
            <p class="fw-bold mb-0">${name}</p>
            <p class="small text-muted">${lastMessage}</p>
        </div>
        ${isOnline ? `<div><i class="text-success fa fa-solid fa-circle"></i></div>` : `` }
    `);

    return li;
}

function fetchUsers(pageIndex, pageSize, onSuccess) {
    $.ajax({
        url: `/api/users?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: onSuccess
    });
}

function LoadUsers() {
    if (isLoading) return;  // Prevent double-triggering
    isLoading = true;

    const skeleton = $("#skeleton-loader");
    skeleton.css("display", "block");

    try {
        fetchUsers(currentUserPage, 10, function (data) {
            const users = data.data.users;
            if (users.length > 0) {
                users.forEach(user => {
                    const item = createUserItem(user.id, "", user.fullName, "", user.isOnline);
                    userList.append(item);
                });
                currentUserPage++; // Prepare for next batch
            }
            else {
                observer.unobserve(trigger[0]);
            }
        });
    }
    catch (error) {
        console.error("Failed to fetch users", error);
    }
    finally {
        isLoading = false; // Reset loading state
        skeleton.css("display", "none");
    }
}

getCurrentUserInfo();

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

connection.on("ReceiveMessage", (result) => {
    const li = createChatItem("", result.data.senderName, result.data.content, result.data.sentAt);
    messageBoard.append(li);

    messageBoard.prop("scrollTop", messageBoard.prop("scrollHeight"));
});

connection.on("UserIsOnline", renderOnlineUser);
connection.on("UserIsOffline", renderOfflineUser);

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

function renderOnlineUser(userId) {
    const userItem = userList.find(`li#${userId} > div`).children("div");
    if (userItem.length > 0) {
        const faOnline = $("<div>").append($("<i>").addClass("text-success fa fa-solid fa-circle"))
        userItem.append(faOnline);
    }
}

function renderOfflineUser(userId) {
    const userItem = userList.find(`li#${userId} > div`).children("div");
    if (userItem.length > 0) {
        userItem.find("div > i.fa").remove();
    }
}

connection.start();



