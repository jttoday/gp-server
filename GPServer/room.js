function __addPlayer(player){
    for (var i = 0; i < this.players.length; ++i) {
        if (this.players[i] == null) {
            this.players[i] = player;
            return;
        }
    }
    this.players.push(playerId);
}

function __leavePlayer(player){
    for (var i = 0; i < this.players.length; ++i) {
        if (this.players[i] != null && this.players[i].clientId == player.clientId) {
            this.players[i] = null;
            return;
        }
    }
}

function Room(){
    this.players = [];
    this.addPlayer = __addPlayer;
    this.leavePlayer = __leavePlayer;
}

module.exports = Room;