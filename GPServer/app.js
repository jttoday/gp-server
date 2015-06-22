var net = require("net");
var config = require('./config.js');
var server = net.createServer(function (sock) {
	console.log('Connected to :' + sock.remoteAddress + ':' + sock.remotePort);
	sock.on('data', function (data) {
		console.log("DATA", sock.remoteAddress + ':' + data);
	});
	sock.on('close', function (data) {
	});
});

server.listen(config.port, function () {
	console.log("server bound on:" + config.port);
});