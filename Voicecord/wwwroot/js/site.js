
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

document.getElementById("SendMessage").addEventListener("click", function (event) {
    var user = document.getElementById("username").textContent;
    var message = document.getElementById("message").value;
    console.log(message);
    var chatId = document.getElementById("chatId").textContent;
    console.log(chatId);
    var linkGroup = document.getElementById("linkGroup").textContent;
    console.log(linkGroup);
    connection.invoke("SendMessage", user, message,linkGroup,chatId );
    event.preventDefault();
    console.log('Message sended');

});
connection.on("ReceiveMessage", function (user, message) {
    var encodedMsg = user + ": " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("discussion").appendChild(li);
});

start();