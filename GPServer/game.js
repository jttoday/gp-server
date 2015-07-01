var Player = require('./player.js');
var Room = require('./room.js');


//------------- public method ---------------//

function __addPlayer(clientId, player) {
    var pos = player.pos;
	this.getRoom(pos).addPlayer(player);
	this.players.push(player);
}

function __addRoom(clientId, room) {
    var pos = room.pos;
    this.map[pos.level][pos.x][pos.y] = room;
    this.rooms.push(room);
}

function __changePlayerStatus(clientId, status) {
	var player = this.findPlayerById(clientId);
	player.status = status;
}


function __playerMoveTo(cliendId, endPos) {
    var player = this.findPlayerById(clientId);
	var currentPos = player.pos;
	this.getRoom(currentPos).leavePlayer(player);
	this.getRoom(endPos).addPlayer(player);
	player.pos = endPos;
	
}

function __getRoom(pos){
	return this.map[pos.level][pos.x][pos.y];
}

function __findPlayerById(clientId) {
	for (var i = 0; i < this.players.length; ++i) {
		if (this.players[i].clientId == clientId) {
			return this.players[i];
		}
	}
}

function __poll(clientId){
	return {
		otherCharacterInfo : this.players,
		roomIsInfo: this.rooms
	};
}

function __changeRoomRotation(pos, rotation) {
	var room = this.getRoom(pos);
	room.rotation = rotation;
}

function Game(width, height){
    this.players = [];
    this.map = [];
    this.rooms = [];
	for (var level = 0; level < 3; ++level) {
		this.map[level] = [];
		for (var i = 0; i < width; ++i) {
			this.map[level][i] = [];
		}
	}
}

module.exports = Game;

Game.prototype.addPlayer = __addPlayer;
Game.prototype.addRoom = __addRoom;
Game.prototype.changePlayerStatus = __changePlayerStatus;
Game.prototype.playerMoveTo = __playerMoveTo;
Game.prototype.getRoom = __getRoom;
Game.prototype.changeRoomRotation = __changeRoomRotation;
Game.prototype.findPlayerById = __findPlayerById;
Game.prototype.poll = __poll;