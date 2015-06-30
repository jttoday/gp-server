var Player = require('./player.js');
var Room = require('./room.js');
var PF = require('pathfinding');



function __findPathAtSameLevel(map, start, end){
	var grid = new PF.Grid(map);
	var finder = new PF.AStarFinder();
	var path = finder.findPath(start.x, start.y, end.x, end.y, grid);
	return path;
}

function __stairPos(currentLevel, targetLevel){
	return this.stairPosList[currentLevel][targetLevel];
}

function __findPathToStair(pos, level, map){
	var stairPos;
	stairPos = this.__stairPos(pos.level, level);
	var pathResult = [];
	for (var i = 0; i < stairPos.length; ++i) {
		pathResult.push(__findPathAtSameLevel(map, pos, stairPos[i]));
	}
	return pathResult;
}

// concatPath : concat two or more path into one long path;
// the path to be concat maybe more than one option
function __concatPath(pathArray){
	var n = pathArray.length;
	var max = 0;
	
	// find the number of pathes
	// max should only be 1 or 2, otherwise, there maybe some problem
	for (var i = 0; i < n; ++i) {
		if (pathArray[i].length > max) {
			max = pathArray[i].length;
		}
	}
	

	var result = [];
	for (var j = 0; j < max; ++j) {
		var path = [];
		for (var i = 0; i < n; ++i) {
			var currentPath;
			// in fact, j should only be 0 or 1
			if (j > pathArray[i].length) {
				currentPath = pathArray[i][0];
			} else {
				currentPath = pathArray[i][j];
			}
			path = path.concat(currentPath);
		}
		result.push(path);
	}
	return result;
}



function __minPath(pathArray){
	var min = pathArray[0].length;
	var idx = 0;
	for (var i = 1; i < pathArray.length; ++i) {
		if (min > pathArray[i].length) {
			min = pathArray[i].length;
			idx = i;
		}
	}
	return pathArray[idx];
}

function __reversePath(pathArray){
	pathArray.forEach(function (path) {
		path.reverse();
	});
	return pathArray;
}

function __convertMap(originMap){

}

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

function __findPath(start, end) {
	var sl = start.level; // start level
	var el = end.level; // end level
	var map1 = convertMap(this.map[sl]);
	var map2 = convertMap[this.map[s2]];
	// case 1 : at the same level
	if (start.level == end.level) {
		var simplaMap = convertMap(this.map[start.level]);
		return __findPathAtSameLvel(simpleMap, start, end);
	}
	// case 2 : at 0 level and at 2 level
	if (Math.abs(sl - el) == 2) {
		var path1 = this.__findPathToStair(start, sl, map1);
		var path2 = __reversePath(this.__findPathToStair(end, el, map2));
		var middlePath = this.PathBetweenStairs;
		if (sl > el) {
			middlePath = __reversePath(middlePath);
		}
		var path = __concatPath([path1, middlePath, path2]);
		return __minPath(path);
	}
	// case 3 : abs(sl - el) == 1
	// one among sl and el equals 1
	if (sl == 1) {
		sl = el;
	} else if (el == 1) {
		el = sl;
	}
	
	var path1 = this.__findPathToStair(start, s1, map1);
	var path2 = this.__findPathToStair(end, el, map2);
	var path = __concatPath([path1, path2]);
	return __minPath(path);
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

Game.prototype.__findPathToStair = __findPathToStair;
Game.prototype.__stairPos = __stairPos;

Game.prototype.addPlayer = __addPlayer;
Game.prototype.addRoom = __addRoom;
Game.prototype.changePlayerStatus = __changePlayerStatus;
Game.prototype.findPath = __findPath;
Game.prototype.playerMoveTo = __playerMoveTo;
Game.prototype.getRoom = __getRoom;
Game.prototype.changeRoomRotation = __changeRoomRotation;
Game.prototype.findPlayerById = __findPlayerById;
Game.prototype.poll = __poll;