const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat")
    .build();

connection.on("ReceiveMessage", (message) => {
    const li = document.createElement("li");
    li.textContent = `${message}`;
    document.getElementById("messageBox").appendChild(li);
});

async function sendMessage(e) {
    const message = document.getElementById("messageInput").value;
    await connection.invoke("SendMessage", message);
}

connection.start();
