var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

var chatId = document.getElementById("tab-0").textContent;
var discussionId = document.getElementById("discussion-0").id;
document.getElementById("SendMessage").addEventListener("click", function (event) {
  
    var message = document.getElementById("message").value;
    console.log(message);
    console.log(chatId);
    var linkGroup = document.getElementById("linkGroup").textContent;
    console.log(linkGroup);
   
    connection.invoke("SendMessage", message, linkGroup, chatId, discussionId);
    event.preventDefault();
    console.log('Message sended');


});
connection.on("ReceiveMessage", function (user, message, disscusionId,dateTime) {
     
    var encodedMsg =  user + ": " + message;
    var li = document.createElement("p");
    li.textContent = encodedMsg;
    console.log(disscusionId);
    var li2 = document.createElement("li");
    li2.textContent = dateTime;
    document.getElementById(disscusionId).appendChild(li2);
    document.getElementById(disscusionId).appendChild(li);
    console.log("appendChildNewMessage");
});


async function change_chat(chat) {
    await connection.invoke("NewConnection", chat);
    
 
}

async function initialize_chats_connection() {
    var chats_count = parseInt(document.getElementById("chats_count").textContent);
    for (let i = 0; i < chats_count; i++) {
        await connection.invoke("NewConnection", 'tab-' + i);
        console.log('tab-' + i);
    }
}

async function start() {
    try {
        await connection.start();
        //var chat = document.getElementById("tab-0").textContent;
        //await connection.invoke("NewConnection", chat);
        await initialize_chats_connection();
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