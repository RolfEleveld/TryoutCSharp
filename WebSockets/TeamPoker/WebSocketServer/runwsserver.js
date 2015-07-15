/*
WebSocket server based on
https://github.com/ncr/node.ws.js
Written By Wayne Ye 6/4/2011
http://wayneye.com
*/

var sys = require("sys"),
    ws = require("./ws");

var wsClients = [], players = [], votedPlayers = [], voteStatus = [];

ws.createServer(function (websocket) {
    websocket.addListener("connect", function (resource) {
        // emitted after handshake
        sys.debug("Client connected on path: " + resource);

        // # Add to our list of wsClients
        wsClients.push(websocket);

        //sys.debug(traverseObj(websocket));

    }).addListener("data", function (data) {
        var clinetMsg = JSON.parse(data);

        switch (clinetMsg.Type) {
            case ClientMsgType.NewParticipaint:
                var newPlayer = clinetMsg.Data;
                sys.debug('New Participaint: ' + newPlayer);
                players.push(newPlayer);

                var gameStatus = new GameStatus();
                gameStatus.Players = players;
                gameStatus.VotedPlayers = votedPlayers;

                var serverMsg = new ServerMessage(ServerMsgType.NewParticipaint, newPlayer);
                broadCast(JSON.stringify(serverMsg));

                // Notify the new client current game status
                var notifyCurrentStatus = new ServerMessage(ServerMsgType.NotifyCurrentStatus, gameStatus);
                wsClients[wsClients.length - 1].write(JSON.stringify(notifyCurrentStatus));

                break;
            case ClientMsgType.NewVoteInfo:
                var newVoteInfo = clinetMsg.Data;
                sys.debug('New VoteInfo: ' + newVoteInfo.PlayerName + ' voted ' + newVoteInfo.VoteValue);

                votedPlayers.push(newVoteInfo.PlayerName);
                voteStatus.push(new VoteInfo(newVoteInfo.PlayerName, newVoteInfo.VoteValue));

                var notifyCurrentStatus = new ServerMessage(ServerMsgType.NewVoteInfo, newVoteInfo.PlayerName);
                broadCast(JSON.stringify(notifyCurrentStatus));
                break;
            case ClientMsgType.ViewVoteResult:
                sys.debug('Broadcast vote result to client(s)..');
                var viewVoteResultMsg = new ServerMessage(ServerMsgType.ViewVoteResult, voteStatus);
                broadCast(JSON.stringify(viewVoteResultMsg));
                break;
            default:
                break;
        }

    }).addListener("close", function () {
        // emitted when server or client closes connection

        for (var i = 0; i < wsClients.length; i++) {
            // # Remove from our connections list so we don't send
            // # to a dead socket
            if (wsClients[i] == websocket) {
                sys.debug("close with client: " + websocket);
                wsClients.splice(i);
                break;
            }
        }
    });
}).listen(8888);

function broadCast(msg) {
    sys.debug('Broadcast server status to all wsClients: ' + msg);
    for (var i = 0; i < wsClients.length; i++)
        wsClients[i].write(msg);
}

var ClientMsgType = {
    NewParticipaint: 'NewParticipaint',
    NewVoteInfo: 'NewVoteInfo',
    ViewVoteResult: 'ViewVoteResult'
};
function ClientMessage(type, data) {
    this.Type = type;
    this.Data = data;
};
var ServerMsgType = {
    NewParticipaint: 'NewParticipaint',
    NewVoteInfo: 'NewVoteInfo',
    NotifyCurrentStatus: 'NotifyCurrentStatus',
    ViewVoteResult: 'ViewVoteResult'
};
function ServerMessage(type, data) {
    this.Type = type;
    this.Data = data;
};
function VoteInfo(playerName, voteValue) {
    this.PlayerName = playerName;
    this.VoteValue = voteValue;
}
function GameStatus() {
    this.Players = [];
    this.VotedPlayers = [];
};


/*Util*/
function traverseObj(obj) {
    var def = '';
    for (var key in obj)
        def += key + ': ' + obj[key];

    return def;
}

/*
var counter = 1;
function sendMsgToClient(ws) {
sys.debug(counter + "");
ws.write(counter++ + "");
setTimeout(sendMsgToClient(ws), 1000);

if (counter == 10) return;
}*/