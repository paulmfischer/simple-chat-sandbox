const net = require('net');
const port = 8124;
var username;

process.stdout.write('\033c');
console.log('what is your username?');

process.stdin.on('data', function getUserName(data) {
  username = data.toString().replace(/(\n|\r)+$/, '');
  connect();
  process.stdin.removeListener('data', getUserName);
});

function connect() {
  const client = net.createConnection({ port }, () => {
    client.setEncoding('utf-8');
    client.write(`username:${username}`);

    process.stdin.on('data', (data) => {
      client.write(data.toString());
    });
  });

  client.on('data', (data) => {
    console.log(`${data.toString()}`);
  });

  client.on('end', () => {
    console.log('disconnected from server');
  });

  client.on('error', (err) => {
    console.log(`Error occured: ${err}`);
  })
}