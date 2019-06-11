'use strict';

const username = 'username:';
var count = 1;

function Client(socket, options) {
    this.socket = socket;
    this.options = options;
    this.name = socket.remoteAddress ? `${socket.remoteAddress}:${socket.remotePort}` : count++;

    this.socket.setEncoding('utf8');
    this.socket.on('end', () => onEnd.call(this));
    this.socket.on('data', (data) => onData.call(this, data));
    this.socket.on('error', (err) => onError.call(this, err));
}

function onData(data) {
    data = data.toString();
    if (data.indexOf(username) >= 0) {
        this.username = data.replace(username, '');
        console.log(`Client::data -> ${this.name} is now known as ${this.username}`);
        this.socket.write(`hello and welcome to simple chat ${this.username}\n`);

        this.options.broadcast(`${this.username} has joined the chat.`, this.socket);
    } else {
        this.options.broadcast(`${this.username} > ${data}`, this.socket);
    }
}

function onEnd() {
    if (this.options.disconnected) {
        this.options.disconnected();
    }

    console.log(`Client::end -> ${this.name} (${this.username}) disconnected`);
}

function onError(err) {
    if (this.options.onError) {
        this.options.onError(this.name);
    }

    console.log(`client error: ${err} - disconnecting`);
    this.options.broadcast(`${this.username} has left the chat`, this.socket);
    this.socket.end();
}

Client.prototype.write = function _write(message) {
    this.socket.write(message);
}

module.exports = Client;