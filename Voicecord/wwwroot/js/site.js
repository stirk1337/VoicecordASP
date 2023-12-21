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


//async function change_chat(chat) {
//    await connection.invoke("NewConnection", chat);
//
//}

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


connection.on("UserDisconnected", function (user, group) {
    delete pc_dict[user];
    var elem = document.getElementById(group + "-" + user);
    elem.parentNode.removeChild(elem);
});

connection.on("ReceiveOfferCandidates", function (user, candidate, sdpMLineIndex, sdpMid, usernameFragment) {
    const candidateDescription = {
        candidate: candidate,
        sdpMLineIndex: sdpMLineIndex,
        sdpMid: sdpMid,
        usernameFragment: usernameFragment
    }
    var pc = pc_dict[user];
    console.log('receivce offer candidates from', user);
    console.log(candidateDescription);
    pc.addIceCandidate(candidateDescription);
});

connection.on("ReceiveAnswerCandidates", function (user, candidate, sdpMLineIndex, sdpMid, usernameFragment) {
    const candidateDescription = {
        candidate: candidate,
        sdpMLineIndex: sdpMLineIndex,
        sdpMid: sdpMid,
        usernameFragment: usernameFragment
    }
    var pc = pc_dict[user];
    console.log(candidateDescription);
    pc.addIceCandidate(candidateDescription);
});

connection.on("ReceiveOffer", async function (user, voice_chat_id, sdp, type) {
    const offerDescription = {
        sdp: sdp,
        type: type
    }
    console.log(offerDescription);

    console.log('initialize user offer', user);
    initializeRemoteStream(user, voice_chat_id);
    var pc = pc_dict[user];
    pc.setRemoteDescription(new RTCSessionDescription(offerDescription));

    const answerDescription = await pc.createAnswer();
    await pc.setLocalDescription(answerDescription);

    const answer = {
        type: answerDescription.type,
        sdp: answerDescription.sdp,
    };

    await connection.invoke("SendAnswer", current_user, user, answer.sdp, answer.type);

    pc.onicecandidate = (event) => {
        var candidate = event.candidate;
        event.candidate && connection.invoke("SendAnswerCandidates", current_user, user, candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
    };
});

connection.on("ReceiveAnswer", function (user, sdp, type) {
    const answerDescription = {
        sdp: sdp,
        type: type
    }
    var pc = pc_dict[user];
    console.log('receive answer from', user);
    console.log(answerDescription);
    pc.setRemoteDescription(new RTCSessionDescription(answerDescription));

});

function initializeRemoteStream(user, voice_chat_id) {
    if (user === current_user) {
        return;
    }

    console.log('initialize user', user);
    let remoteStream = new MediaStream();

    const pc = new RTCPeerConnection(servers);

    try {
        localStream.getTracks().forEach((track) => {
            pc.addTrack(track, localStream);
            console.log('Add local track');
        });
    }
    catch {

    }

    pc.oniceconnectionstatechange = (event) => {
        console.log('ICE connection state change:', pc.iceConnectionState);
    };

    pc_dict[user] = pc;

    pc.ontrack = (event) => {
        event.streams[0].getTracks().forEach((track) => {
            remoteStream.addTrack(track);
            console.log('Add remote track');
        });
    };
    var video_div = document.querySelector('.' + voice_chat_id);
    console.log('Video class:' + voice_chat_id)
    var spanElement = document.createElement("span");
    spanElement.setAttribute("id", voice_chat_id + "-" +user);
    var h3Element = document.createElement("h3");
    h3Element.textContent = user;
    var videoElement = document.createElement("video");
    videoElement.setAttribute("height", "340px");
    videoElement.setAttribute("width", "750px");
    videoElement.setAttribute("id", "remoteVideo");
    videoElement.setAttribute("autoplay", "");
    videoElement.setAttribute("playsinline", "");
    spanElement.appendChild(h3Element);
    spanElement.appendChild(videoElement);
    video_div.appendChild(spanElement);

    videoElement.srcObject = remoteStream;
}

function initializeRemoteStreams(users, voice_chat_id) {
    users.forEach(user => {
        initializeRemoteStream(user, voice_chat_id);
    });
}

connection.on("GetConnectedUsers", function (users, voice_chat_id) {
    console.log(users, voice_chat_id);
    initializeRemoteStreams(users, voice_chat_id);
});


const servers = {
    iceServers: [
        {
            urls: ['stun:stun1.l.google.com:19302', 'stun:stun2.l.google.com:19302'],
        },
    ],
    iceCandidatePoolSize: 10,
};

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


async function initialize_voice_chat(voice_chat_id) {
    if (webcamVideo != null) {
        await disconnect_button(current_voice_chat_id);
    }

    pc_dict = {};

    localStream = null;
    current_user = document.getElementById("username-header").textContent;
    current_voice_chat_id = voice_chat_id;

    let user_name = document.getElementById('user-name-' + voice_chat_id)
    user_name.hidden = false;
    webcamVideo = document.getElementById('webcamVideo-' + voice_chat_id);
    webcamVideo.hidden = false;
    callButton = document.getElementById('callButton-' + voice_chat_id);
    callButton.hidden = true;
    remoteVideo = document.getElementById('remoteVideo-' + voice_chat_id);
    hangupButton = document.getElementById('hangupButton-' + voice_chat_id);
    hangupButton.hidden = false;

    console.log(pc_dict);
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
    webcamVideo.srcObject = localStream;
    console.log('group_name', voice_chat_id);

    await connection.invoke("NewConnection", "voice-" + voice_chat_id);
    await connection.invoke("GetConnectedUsers");


    for (let user in pc_dict) {
        console.log('ITERATING IN', user);
        var pc = pc_dict[user];

        const offerDescription = await pc.createOffer();
        await pc.setLocalDescription(offerDescription);

        const offer = {
            sdp: offerDescription.sdp,
            type: offerDescription.type,
        };
        console.log(offer);
        pc.onicecandidate = (event) => {
            var candidate = event.candidate;
            event.candidate && connection.invoke("SendOfferCandidates", current_user, user, candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
        };

        await connection.invoke("SendOffer", current_user, user, offer.sdp, offer.type);
    };

    hangupButton.disabled = false;
}

let pc_dict = {};

let localStream = null;
let current_user = null;
let current_voice_chat_id = null;

let webcamVideo = null;
let callButton = null;
let remoteVideo = null;
let hangupButton = null;

async function disconnect_button(voice_chat_id) {
    await connection.invoke("VoiceDisconnectButton");
    localStream = null;
    current_voice_chat_id = null;
    webcamVideo.hidden = true;
    webcamVideo = null;
    hangupButton.hidden = true;
    callButton.hidden = false;
    let user_name = document.getElementById('user-name-' + voice_chat_id)
    user_name.hidden = true;
    for (const [key, value] of Object.entries(pc_dict)) {
        delete pc_dict[key];
        var elem = document.getElementById('voice-' + voice_chat_id + "-" + key);
        elem.parentNode.removeChild(elem);
    }
    
}


start();