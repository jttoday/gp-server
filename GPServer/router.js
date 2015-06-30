var config = require('./config.js');
var Game = require('./game.js');


var game = new Game();
function __route(raw) {
	var parsedRaw = JSON.parse(raw);
	var parseData = JSON.parse(parsedRaw.data);
    var clientId = parsedRaw.clientId;
    var result = null;
	console.log("ParsedData is :");
	console.log(parseData);
	//switch (parsedRaw.msgType) {
 //       case config.msgType.JoinGame:
 //           break;
 //       case config.msgType.AddCharacter:
 //           game.addPlayer(clientId, parseData);
 //           break;
 //       case config.msgType.AddRoom:
 //           game.addRoom(clientId, parseData);
 //           break;
 //       case config.msgType.ChangeLevel:
 //           game.changePlayerStatus(clientId, parseData);
 //           break;
 //       case config.msgType.CharacterMove:
 //           game.playerMoveTo(clientId, parseData);
 //           break;
 //       case config.msgType.Poll:
 //           result = game.poll(clientId);
 //           break;
	//	default:
	//		break;

 //   }
    return result;
}

module.exports.route = __route;