var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

var chatId = document.getElementById("tab-0").textContent;

document.getElementById("SendMessage").addEventListener("click", function (event) {
  
    var message = document.getElementById("message").value;
    console.log(message);
    console.log(chatId);
    var linkGroup = document.getElementById("linkGroup").textContent;
    console.log(linkGroup);
    connection.invoke("SendMessage", message, linkGroup, chatId);
    event.preventDefault();
    console.log('Message sended');

});
connection.on("ReceiveMessage", function (user, message) {
    var encodedMsg = user + ": " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("discussion").appendChild(li);
});


async function change_chat(chat) {
    await connection.invoke("NewConnection", chat);
}

async function start() {
    try {
        await connection.start();
        var chat = document.getElementById("tab-0").textContent;
        await connection.invoke("NewConnection", chat);
        console.log("SignalR Connected.");


    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});



start();