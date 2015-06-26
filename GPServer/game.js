var Player = require('./player.js');
var Room = require('./room.js');
var PF = require('pathfinding');


function __addNewPlayer(player){
    this.players.push(player);
}

function __addNewRoom(room){
    this.rooms.push(room);
}

function __findPath(map, start, end){
	var grid = new PF.Grid(map);
	var finder = new PF.AStarFinder();
	var path = finder.findPath(start.x, start.y, end.x, end.y, grid);
	return path;
}


function Game(){
    this.rooms = [];
    this.players = [];

}

var game = new Game();
