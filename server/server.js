const http = require("http");
const Mercury = require('@postlight/mercury-parser');

http.createServer(function(request, response){
    Mercury.parse(request.url.slice(1)).
    then(result =>response.end(JSON.stringify(result)));
    console.log(request.url);
}).listen(3000);
