const { MongoClient, ObjectID } = require("mongodb");
const Express = require("express");
const BodyParser = require('body-parser');

const server = Express();

server.use(BodyParser.json());
server.use(BodyParser.urlencoded({ extended: true }));

const client = new MongoClient("mongodb+srv://fornite:yPp2JIAhuwEYJmTh@cluster0.5h6km.mongodb.net/FORNITE?retryWrites=true&w=majority");
var collection;
/*
server.post("/accounts", async (request, response, next) => {
    try {
        let result = await collection.insertOne(request.body);
        response.send(result);
    } catch (e) {
        response.status(500).send({ message: e.message });
    }
});
*/


/*
server.put("/accounts/:username", async (request, response, next) => {
    try {
        let result = await collection.updateOne(
            { "username": request.params.plummie_tag },
            { "$set": request.body }
        );
        response.send(result);
    } catch (e) {
        response.status(500).send({ message: e.message });
    }
});

server.delete("/accounts/:username", async (request, response, next) => {
    try {
        let result = await collection.deleteOne({ "username": request.params.plummie_tag });
        response.send(result);
    } catch (e) {
        response.status(500).send({ message: e.message });
    }
});

*/

server.listen("3000", async () => {
    try {
        await client.connect();
        collection = client.db("FORNITE").collection("accounts");
        console.log("Listening at :3000...");
       
        
    } catch (e) {
        console.error(e);
    }
});

server.get("/accounts/:username", async (request, response, next) => {
    try {
        let result = await collection.findOne({ "username": request.params.plummie_tag });
        response.send(result);

        return;
    } catch (e) {
        response.status(500).send({ message: e.message });
    }
});