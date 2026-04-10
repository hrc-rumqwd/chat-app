const defaultPageIndex = 1;
const defaultPageOffset = 10;
const hostDomain = "https://localhost:7079";
const maxContentLength = 20;

let isLoading = false;
let currentUserPage = 1;
let currentMessagePage = 1;
let isFetchingOlderMessages = false;
let currentConversationId = null;
let currentConversationIndex = 0;

let observer = undefined;
let topObserver = undefined;

let events = ["contextmenu", "touchstart"];

const userList = $("ul#userList");
const trigger = $("div#loadingTrigger");
const messageBoard = $("ul#messageBoard");
const topSentiel = $("div#topSentinel");

const memberContextMenu = $("#memberContextMenu");

var conversationIdClickedMenuContext = undefined;


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

async function getCurrentUserInfo() {
    await $.ajax({
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
    });
}

async function loadConversations(pageIndex, pageSize, onSuccess) {

    if (isLoading) return;  // Prevent double-triggering
    isLoading = true;

    const skeleton = $("#skeleton-loader");
    skeleton.css("display", "block");

    try {
        await $.ajax({
            url: `/api/conversations?pageIndex=${pageIndex}&pageSize=${pageSize}`,
            type: 'GET',
            xhrFields: {
                withCredentials: true
            },
            success: onSuccess
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

function createMemberItem(conversationId, thumbnail, name, lastMessage, isOnline, onClickItem) {
    const li = $("<li>").addClass("p-2 border-bottom")
        .attr("id", conversationId);

    // Click event for fetching conversation
    li.bind("click", onClickItem);

    const userDisplayItem = $("<div>").addClass("d-flex justify-content-between");
    li.append(userDisplayItem);

    userDisplayItem.html(`
        <div class="pt-1 d-flex flex-row">
            <img src="${thumbnail}" alt="avatar"
            class="rounded-circle d-flex align-self-center me-3 shadow-1-strong" width="60" />
            <div class="container-fluid d-flex flex-column">
                <p class="fw-bold mb-0">${name}</p>
                <p class="small text-muted m-0 last-message-area">${truncateLongContent(lastMessage ?? `Say hello for the first time!`)}</p>
            </div>
        </div>
        ${isOnline ? `<div><i class="text-success fa fa-solid fa-circle"></i></div>` : ``}
    `);

    return li;
}

async function loadConversationMessages(conversationId, pageIndex, pageSize) {
    if (conversationId == null)
        return;

    isFetchingOlderMessages = true;

    // Capture the height before adding new messages
    const previousHeight = messageBoard.prop("scrollHeight");

    // Fetching older messages
    await $.ajax({
        url: `/api/conversations/${conversationId}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: (data) => renderConversationMessages(data),
        error: function (data) {
            //if (data.status == 400)
            //    console.log(data);
        }
    });
}

function createChatItem(thumbnail, senderName, content, sentAt) {
    const li = $('<li>').addClass('d-flex justify-content-between mb-4');
    let sentAtLabel = "";

    // Calculate sent at time with current time
    const current = new Date();
    const sentAtDate = new Date(sentAt);

    sentAtLabel += `${sentAtDate.getHours()}:${sentAtDate.getMinutes()} ${sentAtDate.toLocaleDateString("en-US")}`;

    li.html(`
        <img src="${thumbnail}" alt="avatar"
            class="rounded-circle d-flex align-self-start me-3 shadow-1-strong" width="60">
            <div class="card">
                <div class="card-header d-flex justify-content-between p-3">
                    <p class="fw-bold mb-0">${senderName}</p>
                    <p class="text-muted small mb-0"><i class="far fa-clock"></i> ${sentAtLabel}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">${content}</p>
                </div>
            </div>
    `);

    return li;
}

function StartSignalR() {
    hubConnection.on("ReceiveMessage", (result) => {
        const isSender = isMessageSender(result.data.senderId);

        // Show new message on member list
        updateNewMessageToMemberBoard(result, !isSender);

        // Check user is opening that sender board, then showing message on board too
        updateNewMessageToConversationBoard(result);
    });

    //connection.on("UserIsOnline", renderOnlineUser);
    //connection.on("UserIsOffline", renderOfflineUser);

    hubConnection.on("OnSendMessageError", (data) => {
        // Handle the error (e.g., display an error message to the user)
        console.error(data.data.error);
    });

    hubConnection.start();
}

function updateNewMessageToMemberBoard(result, activeText) {
    const item = userList.find(`li#${result.data.conversationId}`);
    const lastMessageLabel = item.find('p.last-message-area');
    lastMessageLabel.text(truncateLongContent(result.data.content));
    if (activeText)
        lastMessageLabel.addClass("text-active");
}

function updateNewMessageToConversationBoard(result) {
    const li = createChatItem("", result.data.senderName, result.data.content, result.data.sentAt);
    messageBoard.append(li);
    messageBoard.prop("scrollTop", messageBoard.prop("scrollHeight"));
}

async function sendMessage(e) {
    const userInfo = JSON.parse(localStorage.getItem('currentUser'));
    const conversationId = getOpeningConversationId();
    const content = document.getElementById("messageInput").value;

    await hubConnection.invoke("SendMessage", content, userInfo.id, conversationId);

    // Clear the input field after sending the message
    document.getElementById("messageInput").value = "";
}

function getOpeningConversationId() {
    return parseInt(
        userList.find('li')[currentConversationIndex]
            .getAttribute('id')
    );
}

function getConversationInfoById(id) {
    const li = userList.find(`li#${id}`);

    if (li == undefined)
        return { id, name: null };
    const name = li.find('p.fw-bold.b-0').val();
    return { id, name };
}

function renderMemberItems(apiData) {
    const conversations = apiData.data;
    if (conversations.length > 0) {
        conversations.forEach(user => {
            const item = createMemberItem(user.id, user.displayImage, user.displayName, user.lastMessage, user.isOnline, loadConversationByClick);
            userList.append(item);
        });
        currentUserPage++; // Prepare for next batch
    }
    else {
        observer.unobserve(trigger[0]);
    }
}

function appendNewItemToMemberBoard(conversationId, thumbnail, displayName, message, sentAt) {
    const li = createMemberItem(conversationId, thumbnail, displayName, message, false, loadConversationByClick);
    userList.children('li.active-item').removeClass('active-item');
    li.addClass('active-item');
    userList.prepend(li);
}

async function loadConversationByClick() {
    // Active conversation on member board
    if ($(this).hasClass('active-item')) {
        return;
    }

    userList.children('li.active-item').removeClass('active-item');
    const clickedItem = $(this).addClass('active-item');
    const conversationId = parseInt($(this).attr('id'));

    // Load conversation from API into message board
    await loadConversationMessages(conversationId, defaultPageIndex, defaultPageOffset);
}

function renderConversationMessages(apiData) {
    messageBoard.children("li.d-flex.justify-content-between.mb-4").remove();

    if (apiData.data.length > 0) {
        apiData.data
            .forEach(message => {
                // Prepend messages (add them AFTER the sentinel but BEFORE existing messages)
                const item = createChatItem(message.author.avatar, message.author.displayName, message.content, message.sendAt);
                // Insert after the sentinel
                topSentiel.after(item);
            });

        // Adjust scroll position so the user doesn't lose their place
        // New Height - Old Height = The exact distance to maintain the view
        const previousHeight = messageBoard.prop("scrollHeight");
        messageBoard.prop("scrollTop", messageBoard.prop("scrollHeight") - previousHeight);
        currentMessagePage += 1;
    }
    else {
        topObserver.unobserve(topSentiel[0]);
    }

    isFetchingOlderMessages = false;
}

function activeMemberCard(index) {
    const firstItem = userList.find("li");
    if (firstItem == null || firstItem == undefined)
        return;

    if (index > userList.length)
        return;

    firstItem[index].classList.add("active-item");
}

$(document).ready(async function () {
    await getCurrentUserInfo();

    observer = new IntersectionObserver(async (entries) => {
        // If the sentinel is visible, load more!
        if (entries[0].isIntersecting) {
            await loadConversations(currentUserPage, defaultPageOffset, renderMemberItems);
            activeMemberCard(currentConversationIndex);
            const conversationId = userList.children("li").attr("id");
            await loadConversationMessages(conversationId, currentMessagePage, defaultPageOffset, renderConversationMessages);
        }
    }, { threshold: 1.0 });
    observer.observe(trigger[0]);

    topObserver = new IntersectionObserver(async (entries) => {
        if (entries[0].isIntersecting && !isFetchingOlderMessages) {
            await loadConversationMessages(currentConversationId, currentMessagePage, defaultPageOffset, renderConversationMessages, null);
        }
    }, { threshold: 1.0 });
    topObserver.observe(topSentiel[0]);

    //currentConversationId = userList.children("li").attr("id");
    if (currentConversationId != undefined) {
        await loadConversationMessages(currentConversationId, defaultPageIndex, defaultPageOffset, renderConversationMessages, null);
    }

    StartSignalR();

    events.forEach((eventType) => {
        userList[0].addEventListener(
            eventType,
            e => {
                e.preventDefault();

                let clickedElement = document.elementFromPoint(e.clientX, e.clientY);
                let clickedMember = $(clickedElement).closest("li.p-2.border-bottom");
                conversationIdClickedMenuContext = clickedMember.attr("id");

                // x and y position of mouse or touch
                let mouseX = e.clientX || e.touches[0].clientX;
                let mouseY = e.clientY || e.touches[0].clientY;

                // height and width of menu
                let menuHeight = memberContextMenu[0].getBoundingClientRect().height;
                let menuWidth = memberContextMenu[0].getBoundingClientRect().width;

                // widht and height of screen
                let width = window.innerWidth;
                let height = window.innerHeight;

                // If user clicks/touches near right corner
                if (width - mouseX <= 200) {
                    memberContextMenu.css("border-radius", "5px 0 5px 5px");
                    memberContextMenu.css("left", `${width - menuWidth}px`);
                    memberContextMenu.csss("top", `${mouseY}px`);

                    // right bottom
                    if (height - mouseY <= 200) {
                        memberContextMenu.css("top", `${mouseY - menuHeight}px`);
                        memberContextMenu.css("border-radius", "5px 5px 0 5px");
                    }
                }
                // left
                else {
                    memberContextMenu.css("border-radius", "0 5px 5px 5px");
                    memberContextMenu.css("left", mouseX + "px");
                    memberContextMenu.css("top", mouseY + "px");

                    // left bottom
                    if (height - mouseY <= 200) {
                        memberContextMenu.css("top", `${mouseY - menuHeight}px`);
                        memberContextMenu.css("border-radius", "5px 5px 5px 0");
                    }
                }

                // Display the menu
                memberContextMenu.css("visibility", "visible");
            },
            { passive: false }
        );
    });

    memberContextMenu.find("li.menu-item").on("click", function (e) {
        const action = $(this).data("action");
        switch (action) {
            case "create-invitation":
                $.ajax({
                    url: `/api/conversations/${conversationIdClickedMenuContext}/invitation-link`,
                    type: "GET",
                    xhrFields: {
                        withCredentials: true
                    },
                    success: result => {
                        // TODO: Show modal with invitation link
                    }
                });
                break;
            case "remove-conversation":
                break;
        }
    });

    $(document).on("click", function (e) {
        if (!memberContextMenu[0].contains(e.target)) {
            memberContextMenu.css("visibility", "hidden");
        }
    });
});

function showAddConversationModal() {
    // Check the modal is still exists or not
    $.ajax({
        url: "/conversations/create",
        type: "GET",
        success: function (data) {
            $("#conversationModalContainer").html(data);

            $("#addConversationModal").on('shown.bs.modal', function () {
                $('.member-list').select2({
                    dropdownParent: $('#addConversationModal'),
                    width: '100%', // Ensures the select fills the container correctly
                    ajax: {
                        url: `${hostDomain}/api/members`,
                        data: function (params) {
                            var query = {
                                search: params.term,
                                pageIndex: defaultPageIndex,
                                pageSize: defaultPageOffset
                            }
                            return query;
                        },
                        xhrFields: {
                            withCredentials: true
                        },
                        processResults: function (data) {
                            $.each(data.data, (i, m) => {
                                data.data[i]['text'] = m.displayName;
                            });

                            return { results: data.data };
                        }
                    },
                });
            });


            $("#addConversationModal").find("button.btn-primary")
                .on('click', function () {
                    // Create conversation
                    const ids = $('.member-list').val(); // Get selected members
                    createConversation(ids, function (data) {
                        // Create item on member board
                        appendNewItemToMemberBoard(data.data.id, null, data.data.displayName, data.data.lastMessage, new Date());

                        // Load messages for the conversation
                        loadConversationMessages(data.data.id, defaultPageIndex, defaultPageOffset);
                    });

                    // Close the modal
                    $("#addConversationModal").modal('hide');
                });

            $("#addConversationModal").modal('show');
        }
    })
}

function createConversation(userIds, onShowChatBoard) {
    // Create only one to one conversation
    if (userIds.length == 1) {
        $.ajax({
            url: `/api/conversations/${userIds}`,
            type: 'POST',
            xhrFields: {
                withCredentials: true
            },
            success: data => onShowChatBoard(data)
        });
    }

    // Create group conversation
    else if (userIds.length > 1) {
        $.ajax({
            url: `/api/conversations/group`,
            type: 'POST',
            data: { userIds: [...userIds] },
            success: data => onShowChatBoard(data)
        });
    }
    else {
        // TODO: Show error notification instead of console log
        console.error("Please select at least one user to create conversation.");
    }
}


//function renderOfflineUser(userId) {
//    const userItem = userList.find(`li#${userId} > div`).children("div");
//    if (userItem.length > 0) {
//        userItem.find("div > i.fa").remove();
//    }
//}
//function renderOnlineUser(userId) {
//    const userItem = userList.find(`li#${userId} > div`).children("div");
//    if (userItem.length > 0) {
//        const faOnline = $("<div>").append($("<i>").addClass("text-success fa fa-solid fa-circle"))
//        userItem.append(faOnline);
//    }
//}

// SignalR helper functions
function getCurrentLoggedInUser() {
    return JSON.parse(localStorage.getItem('currentUser'));
}

function isMessageReceiver(receiverId) {
    const user = getCurrentLoggedInUser();
    const currentUserId = parseInt(user.id);
    const isReceiver = currentUserId == receiverId;
}

function isMessageSender(senderId) {
    const user = getCurrentLoggedInUser();
    const currentUserId = parseInt(user.id);
    const isReceiver = currentUserId == senderId;
}


// string prototype extension function
String.prototype.truncateLongContent = function () {
    return this.length > maxContentLength ? `${this.slice(0, maxContentLength)}...` : this;
}

function truncateLongContent(content) {
    return content.length > maxContentLength ? `${content.slice(0, maxContentLength)}...` : content;
}