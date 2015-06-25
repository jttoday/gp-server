function __addPlayer(playerId){
    for (var i = 0; i < this.players.length; ++i) {
        if (this.players[i] == -1) {
            this.players[i] = playerId;
            return;
        }
    }
    this.players.push(playerId);
}

function __leavePlayer(playerId){
    for (var i = 0; i < this.players.length; ++i) {
        if (this.players[i] == playerId) {
            this.players[i] = -1;
        }
    }
}

function Room(){
    this.players = [];
    this.addPlayerId = __addPlayer;
    this.leavePlayerId = __leavePlayer;
}

module.exports = Room;