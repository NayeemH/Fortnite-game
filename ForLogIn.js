const jwt = require("jsonwebtoken"); //calling jsonwebtoken here
const dotenv = require ("dotenv"); //calling dotenv

const express = require("express");
const mongoose = require("mongoose");
const bcrypt = require("bcrypt");
const router = express.Router();
const userSchema = require("./schemas/userSchema");
const User = new mongoose.model("User", userSchema);
const app = express();

const bodyParser = require('body-parser');

require('./model/Account');
const Account = mongoose.model('accounts');

// parse application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: false }))

//app initialization 
dotenv.config()


console.log("Listening on authentication");

//
app.get('/account', async (req, res) => {

    const { rUsername, rPassword } = req.query;
    if(rUsername == null || rPassword == null)
    {
        res.send("Invalid credentials1");
        return;
    }
    

    console.log("okk");
    var userAccount = await Account.findOne({ username: rUsername});
    console.log(userAccount);
    if(userAccount == null){
        // Create a new account
        console.log("Create new account...")

        var newAccount = new Account({
            username : rUsername,
            password : rPassword,

            lastAuthentication : Date.now()
        });
        await newAccount.save();

        res.send(newAccount);
        return;
    } else {
        if(rPassword == userAccount.password){
            userAccount.lastAuthentication = Date.now();
            await userAccount.save();

            console.log("Retrieving account...")
            res.send(userAccount);
            return;
        }
    }

    res.send("Invalid credentials2");
    return;
});


//
app.get("/login",async(req,res)=>{   //In login section we r using jwt
   // console.log(user);
    
    try{
        console.log("finding user...");
    const user = await User.find({username: req.body.username }); // finding the given user  name
    if(user && user.length >0)
    {
     
        console.log(user);
        const isValidPass = await bycrypt.compare(req.body.password,user[0].password);

        if(isValidPass)
        {
            //Here we will generate Token so install jsonwebtoken in the environment first

            const token = jwt.sign({
                username: user[0].username,
                userId:user[0]._id,

            },process.env.JWT_SECRET_TOKEN,{  //install dotenv in the environment and here we call the secret key
                expiresIn: '1h' //session validation for token
            } ); 

            res.status(200).json({
                "access_token": token,
                "message": "Login successful!" //showing user the token
            })



        }

        else{

            res.status(401).json({
                "error": "Authentication Failed!"
            });
        }

    }


    else                           //If not matched with server data
    {
        res.status(401).json({
            "error": "Authentication Failed!"
        });
    }
}
catch
{
    res.status(401).json({
        "error": "Authentication Failed!"
    });
}



res.send("Invalid credentials2");
return;

});

app.listen(13756, () => {
    console.log("app listening at port 13756");
  });

//before uploading .env file make it .gitignore