var net = require("net");
var config = require('./config.js');
var router = require('./router.js');
var server = net.createServer(function (sock) {
	console.log('Connected to :' + sock.remoteAddress + ':' + sock.remotePort);
	sock.on('data', function (data) {
		var result = router.route(data);
		if (result) {
			sock.write(result);
		}
	});
	sock.on('close', function (data) {
		console.log("Colsed!");
	});
	sock.on('error', function (err) {
		console.log(err);
	});
});

server.listen(config.port, function () {
	console.log("server bound on:" + config.port);
});