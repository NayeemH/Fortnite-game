const express = require('express');
const keys = require('./config/keys.js');
const bodyParser = require('body-parser');

const app = express();


//  parse application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: false }))


//  Including mongoose for using object data modeling library for MongoDB
//  Conneting with Database
//  key.mongoURI is from ./config/keys.js which contains Atlas cluster URI and PORT
const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI, {useNewUrlParser: true, useUnifiedTopology: true});
console.log(keys.mongoURI);


//  Setup database models using mongoose
require('./model/Account');
const Account = mongoose.model('accounts');


//  authenticationRoutes.js is included here.
//  This file contains account creating and login
require('./routes/authenticationRoutes')(app);


//  stablishing server on keys.port
app.listen(keys.port, () => {
    console.log("Listening on " + keys.port);
});
