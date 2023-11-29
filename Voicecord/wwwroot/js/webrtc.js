var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();


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

connection.on("ReceiveOffer", async function (user, sdp, type) {
    const offerDescription = {
        sdp: sdp,
        type: type
    }
    console.log(offerDescription);

    console.log('initialize user offer', user);
    let remoteStream = new MediaStream();

    const pc = new RTCPeerConnection(servers);

    localStream.getTracks().forEach((track) => {
        pc.addTrack(track, localStream);
        console.log('Add local track');
    });

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

    var spanElement = document.createElement("span");
    var h3Element = document.createElement("h3");
    h3Element.textContent = "Remote Stream";
    var videoElement = document.createElement("video");
    videoElement.setAttribute("id", "remoteVideo");
    videoElement.setAttribute("autoplay", "");
    videoElement.setAttribute("playsinline", "");
    spanElement.appendChild(h3Element);
    spanElement.appendChild(videoElement);
    document.body.appendChild(spanElement);

    videoElement.srcObject = remoteStream;

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

function initializeRemoteStreams(users, current_user) {
    users.forEach(user => {
        if (user === current_user) {
            return;
        }

        console.log('initialize user', user);
        let remoteStream = new MediaStream();

        const pc = new RTCPeerConnection(servers);

        localStream.getTracks().forEach((track) => {
            pc.addTrack(track, localStream);
            console.log('Add local track');
        });

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

        var spanElement = document.createElement("span");
        var h3Element = document.createElement("h3");
        h3Element.textContent = "Remote Stream";
        var videoElement = document.createElement("video");
        videoElement.setAttribute("id", "remoteVideo");
        videoElement.setAttribute("autoplay", "");
        videoElement.setAttribute("playsinline", "");
        spanElement.appendChild(h3Element);
        spanElement.appendChild(videoElement);
        document.body.appendChild(spanElement);

        videoElement.srcObject = remoteStream;
    });
}

connection.on("GetConnectedUsers", function (users) {
    console.log(users);
    initializeRemoteStreams(users, current_user);
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


//const pc = new RTCPeerConnection(servers);
let pc_dict = {};
//pc_dict.set('stirk', new RTCPeerConnection(servers));

//pc.oniceconnectionstatechange = (event) => {
//    console.log('ICE connection state change:', pc.iceConnectionState);
//};


let localStream = null;
//let remoteStream = null;
let current_user = document.getElementById("username-header").textContent;

const webcamButton = document.getElementById('webcamButton');
const webcamVideo = document.getElementById('webcamVideo');
const callButton = document.getElementById('callButton');
const callInput = document.getElementById('callInput');
const answerButton = document.getElementById('answerButton');
const remoteVideo = document.getElementById('remoteVideo');
const hangupButton = document.getElementById('hangupButton');


webcamButton.onclick = async () => {
    //localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
    //remoteStream = new MediaStream();

    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
    webcamVideo.srcObject = localStream;

    await connection.invoke("NewConnection", current_user);
    await connection.invoke("GetConnectedUsers");

    //localStream.getTracks().forEach((track) => {
    //    pc.addTrack(track, localStream);
    //    console.log('Add local track');
    //});

    //pc.ontrack = (event) => {
    //    event.streams[0].getTracks().forEach((track) => {
    //        remoteStream.addTrack(track);
    //        console.log('Add remote track');
    //    });
    //};

    //webcamVideo.srcObject = localStream;
    //remoteVideo.srcObject = remoteStream;

    callButton.disabled = false;
    answerButton.disabled = false;
    webcamButton.disabled = true;
};

callButton.onclick = async () => {
    console.log(pc_dict);
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
    //pc.onicecandidate = (event) => {
    //    var candidate = event.candidate;
    //    event.candidate && connection.invoke("SendOfferCandidates", candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
    //};

    //const offerDescription = await pc.createOffer();
    //await pc.setLocalDescription(offerDescription);

    //const offer = {
    //    sdp: offerDescription.sdp,
    //    type: offerDescription.type,
    //};

    //await connection.invoke("SendOffer", offer.sdp, offer.type);
  
    hangupButton.disabled = false;
};

//answerButton.onclick = async () => {
//    pc.onicecandidate = (event) => {
//        var candidate = event.candidate;
//        event.candidate && connection.invoke("SendAnswerCandidates", candidate.candidate, candidate.sdpMLineIndex, candidate.sdpMid, candidate.usernameFragment);
//    };
//
//
//    const answerDescription = await pc.createAnswer();
//    await pc.setLocalDescription(answerDescription);
//
 //   const answer = {
//        type: answerDescription.type,
//        sdp: answerDescription.sdp,
//    };
//
//    await connection.invoke("SendAnswer", answer.sdp, answer.type);
//};


start();