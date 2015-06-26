var Player = require('./player.js');
var Room = require('./room.js');
var PF = require('pathfinding');


function __addPlayer(pos, player){
    this.map[pos.x][pos.y].addPlayer(player);
    this.players.push(player);
}

function __addRoom(pos, room) {
    this.map[pos.x][pos.y] = room;
}

function __changePlayerStatus(clientId, status){
    var player = this.findPlayerById(clientId);
    player.status = status;
}

function __findPlayerById(clientId){
    for (var i = 0; i < this.players.length; ++i) {
        if (this.players[i].clientId == clientId) {
            return this.players[i];
        }
    }
}

function __findPath(map, start, end){
	var grid = new PF.Grid(map);
	var finder = new PF.AStarFinder();
	var path = finder.findPath(start.x, start.y, end.x, end.y, grid);
	return path;
}

function __playerMoveTo(player, endPos){
    var currentPos = player.pos;
    this.map[currentPos.x][currentPos.y].leavePlayer(player);
    this.map[endPos.x][endPos.y].addPlayer(player);
    player.pos = endPos;
}


function Game(width, height){
    this.players = [];
    this.map = [];
    for (var i = 0; i < width; ++i) {
        this.map[i] = new Array(height);
    }
}

var game = new Game();
