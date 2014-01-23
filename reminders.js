/*
 * Reminders Application for Mycroft
 * 
 */

//Connect to central mycroft server

var app = require('./app.js');
var fs = require('fs');
var cronJob = require('cron').CronJob;

var client = app.connectToMycroft();
app.sendManifest(client, './app.json');

var verified = false; //Set to true when APP_MANIFEST_OKAY received

client.on('data', function (data) {
  parsed = app.parseMessage(data);
  //Check the type of ths message
  if (parsed.type === 'APP_MANIFEST_OK' || 'APP_MANIFEST_FAIL') {
    var dependencies = app.manifestCheck(data);
    verified = true;

  } else if (parsed.type === 'MSG_QUERY') {
    console.log('Query received');

  } else if (parsed.type === 'MSG_QUERY_SUCCESS') {
    console.log('Query successful');

  } else if (parsed.type === 'MSG_QUERY_FAIL') {
    console.error('Query Failed.');
    throw parsed.data.message;

  } else {
    console.log('Message Receieved');
    console.log(' - Type: ' + parsed.type);
    console.log(' - Message:' + JSON.stringify(parsed.data));
  }
  

  if(dependencies){
    if(dependencies.logger == 'up'){
      app.up(client);
    }
  }
});

client.on('end', function() {
  console.log('client disconnected');
});

var trashMessageBank = loadMessages('messages.txt');
var trashMessage = "";

new cronJob('* 30 10-17 * * *', function(){
  if (verified) {
    trashMessage = getRandomMessage(trashMessageBank);
    informRoom(trashMessage);
  }
}, null, true);

function informRoom(message){
  var msg = ('[INFO] [' + getTime() + '] Sending trash 30 query');
  console.log(msg);
  app.broadcast(client, msg);
  app.query(client, 'tts', 'stream', {text: message, targetSpeaker: "speakers"}, 30);
}

function getTime(){
  var date = new Date();
  var parts = /(\d+)-(\d+)-(\d+)T(.*)\.(.*)/.exec(date.toISOString());
  return (
    parts[2] + '/' +
    parts[3] + '/' +
    parts[1] + ' ' +
    parts[4]
  );
}

function getRandomMessage(messageArray){
  var randomNumber=Math.floor(Math.random()*messageArray.length)
  return messageArray[randomNumber];
}

//Loads messages from the text file specified by 'path'
function loadMessages(path){
  var array = fs.readFileSync(path).toString().split("\n");
  for(i in array) {
      console.log(array[i]); //Temporary Validation
  }
  return array;
}