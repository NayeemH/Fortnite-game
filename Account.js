const mongoose = require('mongoose');
const { Schema } = mongoose;

//  Data model
const accountSchema = new Schema({
    username: String,
    password: String,
    gameid : String,

    lastAuthentication: Date,
});

mongoose.model('accounts', accountSchema);
