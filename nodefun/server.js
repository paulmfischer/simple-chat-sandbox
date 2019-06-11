const net = require('net');
const Client = require('./client');
const port = 8124;
var clients = [];

process.stdout.write('\033c');

const server = net.createServer((socket) => {
  clients.push(new Client(socket, {
    disconnected: () => {
      console.log('end called');
    },
    broadcast: broadcast,
    onError: (name) => {
      clients = clients.filter(client => client.name !== name);
    }
  }));
});

server.on('error', (err) => {
  throw err;
});

server.listen(port, () => {
  console.log(`server listening on ${port}`);
});

process.stdin.on('data', (data) => {
  broadcast(`administrator > ${data.toString()}`);
})

// Send a message to all clients
function broadcast(message, sender) {
  clients.forEach((client) => {
    // Don't want to send it to sender
    if (client.name === sender.name) {
      return;
    }
    
    client.write(message);
  });
  
  // Log it to the server output too
  console.log(message);
}