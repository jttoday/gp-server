var Player = require('./player.js');
var Room = require('./room.js');


function __addNewPlayer(player){
    this.players.push(player);
}

function __addNewRoom(room){
    this.rooms.push(room);
}


function Game(){
    this.rooms = [];
    this.players = [];

}

var game = new Game();
