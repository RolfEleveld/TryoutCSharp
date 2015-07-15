(function (window) {
    var TeamPoker = TeamPoker || function () { };
    TeamPoker.CurrentPlayerName = '';
    TeamPoker.WsClient = null;
    TeamPoker.VoteInfo = function (playerName, voteValue) {
        this.PlayerName = playerName;
        this.VoteValue = voteValue;
    }
    TeamPoker.voteResult = [];
    TeamPoker.MessageType = {
        NewParticipaint: 'NewParticipaint',
        NewVoteInfo: 'NewVoteInfo',
        ViewVoteResult: 'ViewVoteResult'
    };
    TeamPoker.ClientMessage = function (type, data) {
        this.Type = type;
        this.Data = data;
    };
    TeamPoker.ServerMsgType = {
        NewParticipaint: 'NewParticipaint',
        NewVoteInfo: 'NewVoteInfo',
        NotifyCurrentStatus: 'NotifyCurrentStatus',
        ViewVoteResult: 'ViewVoteResult'
    };
    TeamPoker.GameStatus = function () {
        this.Players = [];
        this.VotedPlayers = [];
    };

    TeamPoker.initUI = function () {
        $('.poker').click(
        function (e) {
            var chosenPoker = $(this);
            var value = chosenPoker.text();
            TeamPoker.vote(value);
        }
    );
        $('#btnLogin').click(TeamPoker.login);
        $('#loginForm').keydown(function (evt) {
            if (evt.keyCode == 13) TeamPoker.login();
        });
    }
    TeamPoker.login = function () {
        $('#mask').hide();
        $('#loginForm').hide();

        TeamPoker.CurrentPlayerName = $('#txtNickname').val();

        // Check Admin, hardcoded for now
        if (TeamPoker.CurrentPlayerName == 'Wayne') $('#btnViewResult').css('display', 'block');

        TeamPoker.connectToWsServer();
    };
    TeamPoker.connectToWsServer = function () {
        // Init Web Socket connect
        var WSURI = "ws://16.158.149.139:8888";
        TeamPoker.WsClient = new WebSocket(WSURI);

        TeamPoker.WsClient.onopen = function (evt) {
            console.log('Successfully connected to WebSocket server.');
            TeamPoker.joinGame();
        };
        TeamPoker.WsClient.onclose = function (evt) {
            console.log('Connection closed.');
        };
        TeamPoker.WsClient.onmessage = function (evt) {
            console.log('Retrived msg from server: ' + evt.data);
            TeamPoker.updateGameStatus(evt.data);
        };
        TeamPoker.WsClient.onerror = function (evt) {
            console.log('An error occured: ' + evt.data);
        };
    };
    TeamPoker.joinGame = function () {
        var joinGameMsg = new TeamPoker.ClientMessage(TeamPoker.MessageType.NewParticipaint, TeamPoker.CurrentPlayerName);

        TeamPoker.WsClient.send(JSON.stringify(joinGameMsg));
    }
    TeamPoker.vote = function (value) {
        var voteInfo = new TeamPoker.VoteInfo(TeamPoker.CurrentPlayerName, value);
        var voteMsg = new TeamPoker.ClientMessage(TeamPoker.MessageType.NewVoteInfo, voteInfo);
        TeamPoker.WsClient.send(JSON.stringify(voteMsg));

        $('#pokerbar').html('<p>Thank you for voting:)</p>');
    };

    TeamPoker.updateGameStatus = function (data) {
        var serverMsg = JSON.parse(data);
        var voteBar = $('#votebar');
        var playerBar = $('#participaints');
        var playerSpliter = ', '

        switch (serverMsg.Type) {
            case TeamPoker.ServerMsgType.NewParticipaint:
                if (playerBar.html() == "") playerBar.html(serverMsg.Data);
                else playerBar.html(playerBar.html() + playerSpliter + serverMsg.Data);
                break;
            case TeamPoker.ServerMsgType.NewVoteInfo:
                var $newVotedPoker = $('<div class="votedPoker"><div class="poker"></div><span>' + serverMsg.Data + '</span></div>');
                voteBar.append($newVotedPoker);
                setTimeout(function () { $newVotedPoker.css('opacity', 1); }, 50);
                break;
            case TeamPoker.ServerMsgType.NotifyCurrentStatus:
                var players = serverMsg.Data.Players;
                $('#participaints').html(players.join(playerSpliter));

                var votedPlayers = serverMsg.Data.VotedPlayers;
                for (var i = 0; i < votedPlayers.length; i++) {
                    var $votedPoker = $('<div class="votedPoker"><div class="poker"></div><span>' + votedPlayers[i] + '</span></div>');
                    $votedPoker.css('opacity', 1);
                    voteBar.append($votedPoker);
                }

                break;
            case TeamPoker.ServerMsgType.ViewVoteResult:
                $('#mask').show();
                $('#voteResultWrapper').show();
                var voteResultPanel = $('#voteResultPanel');

                var voteStatus = serverMsg.Data;
                function animateVotedPoker(poker) {
                    poker.css('opacity', 1);
                    poker.css('webkitTransform', 'rotateY(360deg)');
                }

                for (var key in voteStatus) {
                    var $votedPoker = $('<div class="votedPoker resultPoker"><div class="poker">' + voteStatus[key].VoteValue + '</div><span>' + voteStatus[key].PlayerName + '</span></div>');

                    if (voteStatus[key].VoteValue == '0' || voteStatus[key].VoteValue == '?')
                        $votedPoker.bind('webkitTransitionEnd', function () { $(this).css('margin-top', '10px'); $(this).css('color', 'red') }); // Vibrates aliens $(this).vibrate('y', 10, 2, 100);

                    voteResultPanel.append($votedPoker);
                    TeamPoker.voteResult.push($votedPoker);
                }

                for (var i = 0; i < TeamPoker.voteResult.length; i++) {
                    (function (p) {
                        setTimeout(function () { animateVotedPoker(p); }, 100);
                    })(TeamPoker.voteResult[i]);
                };

                break;
            default:
                break;
        }
    };

    TeamPoker.viewVoteResult = function () {
        var viewResultMsg = new TeamPoker.ClientMessage(TeamPoker.MessageType.ViewVoteResult, null);
        TeamPoker.WsClient.send(JSON.stringify(viewResultMsg));
    };
    TeamPoker.checkAlienVote = function (voteValue) {
        if (voteValue == '0' || voteValue == '?')
            return true;
    };

    TeamPoker.Util = {};
    TeamPoker.Util.BuildStr = function () {
        var tmp = [];
        for (idx in arguments) {
            tmp.push(arguments[idx]);
        }

        return tmp.join('');
    }

    window.TeamPoker = TeamPoker;
})(window);


$(document).ready(function () {
    TeamPoker.initUI();
    $('#txtNickname').focus();
    $('#btnViewResult').click(TeamPoker.viewVoteResult);
});

