var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();


connection.on("ReceiveOfferCandidates", function (candidate, sdpMLineIndex, sdpMid, usernameFragment) {
    const candidateDescription = {
        candidate: candidate,
        sdpMLineIndex: sdpMLineIndex,
        sdpMid: sdpMid,
        usernameFragment: usernameFragment
    }
    console.log(candidateDescription);
    pc.addIceCandidate(candidateDescription);
});

connection.on("ReceiveAnswerCandidates", function (candidate, sdpMLineIndex, sdpMid, usernameFragment) {
    const candidateDescription = {
        candidate: candidate,
        sdpMLineIndex: sdpMLineIndex,
        sdpMid: sdpMid, 
        usernameFragment: usernameFragment
    }
    console.log(candidateDescription);
    pc.addIceCandidate(candidateDescription);
});

connection.on("ReceiveOffer", function (sdp, type) {
    const offerDescription = {
        sdp: sdp,
        type: type
    }
    console.log(offerDescription);
    pc.setRemoteDescription(new RTCSessionDescription(offerDescription));
});

connection.on("ReceiveAnswer", function (sdp, type) {
    const answerDescription = {
        sdp: sdp,
        type: type
    }
    console.log(answerDescription);
    pc.setRemoteDescription(new RTCSessionDescription(answerDescription));
    
});

connection.on("GetConnectedUsers", function (users) {
    console.log(users);

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


const pc = new RTCPeerConnection(servers);
//let pc_dict = new Map();
//pc_dict.set('stirk', new RTCPeerConnection(servers));

pc.oniceconnectionstatechange = (event) => {
    console.log('ICE connection state change:', pc.iceConnectionState);
};


let localStream = null;
let remoteStream = null;

const webcamButton = document.getElementById('webcamButton');
const webcamVideo = document.getElementById('webcamVideo');
const callButton = document.getElementById('callButton');
const callInput = document.getElementById('callInput');
const answerButton = document.getElementById('answerButton');
const remoteVideo = document.getElementById('remoteVideo');
const hangupButton = document.getElementById('hangupButton');


webcamButton.onclick = async () => {
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
    remoteStream = new MediaStream();

    await connection.invoke("NewConnection", 'stirk');
    await connection.invoke("GetConnectedUsers");

    localStream.getTracks().forEach((track) => {
        pc.addTrack(track, localStream);
        console.log('Add local track');
    });

    pc.ontrack = (event) => {
        event.streams[0].getTracks().forEach((track) => {
            remoteStream.addTrack(track);
            console.log('Add remote track');
        });
    };

    webcamVideo.srcObject = localStream;
    remoteVideo.srcObject = remoteStream;

    callButton.disabled = false;
    answerButton.disabled = false;
    webcamButton.disabled = true;
};

callButton.onclick = async () => {

    pc.onicecandidate = (event) => {
        var candidate = event.candidate;
        event.candidate && connection.invoke("SendOfferCandidates", candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
    };

    const offerDescription = await pc.createOffer();
    await pc.setLocalDescription(offerDescription);

    const offer = {
        sdp: offerDescription.sdp,
        type: offerDescription.type,
    };

    await connection.invoke("SendOffer", offer.sdp, offer.type);

  
    hangupButton.disabled = false;
};

answerButton.onclick = async () => {
    pc.onicecandidate = (event) => {
        var candidate = event.candidate;
        event.candidate && connection.invoke("SendAnswerCandidates", candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
    };


    const answerDescription = await pc.createAnswer();
    await pc.setLocalDescription(answerDescription);

    const answer = {
        type: answerDescription.type,
        sdp: answerDescription.sdp,
    };

    await connection.invoke("SendAnswer", answer.sdp, answer.type);
};


start();