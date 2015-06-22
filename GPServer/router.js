var config = require('./config.js');


function __route(data) {
	var parsedData = JSON.parse(data);
	switch (parsedData.msgType) {
		case config.msgType.CreateGame:
			break;
		case config.msgType.JoinGame:
			{
				var gameId = parsedData.gameId;
				var clientId = parsedData.clientId;
				break;
			}
		default:
			break;

	}
	
}