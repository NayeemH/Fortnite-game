const mongoose = require('mongoose');// for using object data modeling library for MongoDB
const Account = mongoose.model('accounts');// for creating user data to upload on Database
const bcrypt = require("bcrypt");// for hashing password
const jwt = require("jsonwebtoken");// for generating jwt token
const dotenv = require("dotenv"); // for JWT secrete code in env access
//const checkLogin = require("/middleware/middleware");


dotenv.config();
const crypto = require('crypto');

//  Using express app which is passed from server.js
module.exports = app => {

    console.log("Listening on authentication");


    //  Routes for login
    //  User End to Server data is passed through web request(req) query 
    //  Server End to User End data is passed through web response(res)
    app.get('/account/login',cehckLogin, async (req, res) => {

        console.log("req: "+req.rGameid+" "+req.rPassword);
        
        var response = {};

      //  Getting querry values in variables
      //  const rGameid = req.rGameid;
      //  const rPassword = req.rPassword;
        const { rGameid, rPassword } = req.query;

        console.log("pass- "+rPassword+" id- "+rGameid);

        //  Checking valid input field. It is also checked in unity login.cs.
        if(rGameid == null || rPassword== null)
        {

            //  Sending response to User End for null input field
            response.code = 1;
            response.msg = "Invalid credentials";
            res.send(response);
            return;
        }

        //  Try is for using await. Server may crash or infined loading if await does not response
        try{
        console.log("finding user...");

        //  Finding user account from MongoDB matching by rGameid
        var userAccount = await Account.findOne({ gameid: rGameid}, 'username adminFlag password');


        //  Checking whether user is found. Password will be compared if any user is matched by rGameid
        if(userAccount != null){

            //  Comparing user end password(Not encrypted) with encrypted password stored in MongoDB user Account
            const isValidPass = await bcrypt.compare(rPassword,userAccount.password);

                console.log(rPassword+" compares "+userAccount.password+" isValidPass "+isValidPass);
                if(isValidPass){

                    //  Creating JWT access token
                    const token = jwt.sign({
                        gameid: rGameid,
                       // password : userAccount.password,

                    //  JWT_SECRET_TOKEN is taken form .env
                    },process.env.JWT_SECRET_TOKEN,{ 
                       
                        //  Session validation time 
                        expiresIn: '1h'
                    } ); 
                    

                    //  Sending response for successful login 
                    res.status(200).json({
                        "access_token": token,
                        "message": "Login successful!"
                    });

                    //  Last authentication time is updated and saved in MongoDB
                    userAccount.lastAuthentication = Date.now();
                    await userAccount.save();

                    //  Sending response of finding user account
                    response.code = 0;
                    response.msg = "Account found";
                    response.data = ( ({rGameid, adminFlag}) => ({ rGameid, adminFlag }) )(userAccount);
                    res.send(response);
                    return;
                }
                else{

                    //  Sending response for user password not matched
                    response.code = 1;
                    response.msg = "Invalid credentials";
                    res.send(response);
                    return;
                }
            }
        
        else{

            //  Sending response for not matching rGameid in MongoDB
            response.code = 1;
            response.msg = "Invalid credentials";
            console.log(userAccount);
            res.send(response);
            return;
        }
    }
    catch
    {
       /*
        //  Sending response for failed on try. 
        console.log("Finding Error!");
        res.status(401).json({
            "error": "Authentication Failed!"
        });
        */
        
    }
        
 });


    //  Routes for user account creation
    //  User End to Server data is passed through web request(req) query 
    //  Server End to User End data is passed through web response(res)
    app.get('/account/create', async (req, res) => {

        var response = {};
        
        //  Getting querry values in variables
        const { rUsername, rPassword, rGameid } = req.query;
        console.log(req.query);
        console.log(rUsername+ " "+rPassword+" "+rGameid);

        //  Checking valid input field. It is also checked in unity login.cs. 
        if(rUsername == null || rUsername.length < 3 || rUsername.length > 24)
        {

            //  Sending response for invalid user input
            response.code = 1;
            response.msg = "Invalid credentials";
            console.log(rUsername+" "+rPassword+" "+rGameid);
            res.send(response);
            return;
        }
        
        //  Generating encrypted password 
        const hashedPassword = await bcrypt.hash(rPassword, 10);

        console.log(rPassword);
  
        //  Finding and checking gameid is already available on database.
        //  If gameid is available then it will ask for another gameid to user
        var userAccount = await Account.findOne({ username: rUsername},'_id');

        //  Account will be successfully created if gameid is not available already
        if(userAccount == null){
            console.log("Create new account...")

                    //  Creating and assigning data on datamodel of new user
                    var newAccount = new Account({
                        username : rUsername,
                        password : hashedPassword,
                        gameid : rGameid,
        
                        lastAuthentication : Date.now()
                    });
                    console.log("Account Creation Successful: "+newAccount);
                    
                    //  Saving new user account and sending new account data to user end
                    await newAccount.save();
                    res.send(newAccount);
                    return;

        } else {

            //  Sending response if gameid is already available.
            response.code = 2;
            response.msg = "Username is already taken";
            res.send(response);
        }
        
        return;

    }); 


    
    function cehckLogin(req,res,next)
{
    /*request will come at Header from user, when the user will hit the route here will be a authorization section
    where there will be Bearer adhakjw23n.. (jwt Token)*/

    const {authorization} = req.headers; //it is calling from authorization
    try{
        
        console.log("JWT Authorization: " + req.headers);

        const token = authorization.split(' ')[1]; //spliting the word Bearer and taking the token

        console.log("token: "+token);
        const decoded = jwt.verify(token,process.env.JWT_SECRET_TOKEN); //jwt.verify will verify the token
        console.log("decoded: "+decoded);
        const {gameid, password} = decoded;
        req.rGameid = gameid;
        req.rPassword = password;
        console.log(req);
      //  req.rPassword = password;
      // req.userId = userId; //in the next routes if the username or userid is needed
        next(); //after this it will go to next
       
    }
    catch
    { 
        console.log("Authentication Failed!");

        next("Authentication Failed!");
       
    }

}

}
