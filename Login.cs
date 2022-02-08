using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;

public class Login : MonoBehaviourPunCallbacks
{

        public GameObject connectionPanel;

    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,24})";

    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/account/login";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:13756/account/create";


    [SerializeField] private TextMeshProUGUI alertTextLogIn;
    [SerializeField] private TextMeshProUGUI alertTextSignUp;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button createButton;
    [SerializeField] private InputField usernameInputFieldLogin;
    [SerializeField] private InputField passwordInputFieldLogin;
    [SerializeField] private InputField usernameInputFieldSignUp;
    [SerializeField] private InputField gameIDInputFieldSignUp;
    [SerializeField] private InputField gameIDInputFieldLogIn;
    [SerializeField] private InputField passwordInputFieldSignUp;


      void Start()
    {
        while(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Photon Connection sent");
        }
    }
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Update()
    {
        //connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
        Debug.Log("Connection Status: "+ PhotonNetwork.NetworkClientState);
    }
    #region public methods
    public void SetPlayerName(string playername) ///making a public class for player input section
    {
        if (string.IsNullOrEmpty(playername)) ///Checking if the player name is empty or not
        {
            Debug.Log("Player Name is Empty.");
            return;
        }
        PhotonNetwork.LocalPlayer.NickName = playername; //else connecting the name to the server
    }
    
    public void OnClickEnterGameJoinRandomRoom() ///function for joinning random room when Enter Game is Clicked
    {
        
       
        PhotonNetwork.JoinRandomRoom();
        connectionPanel.SetActive(true);
    }

    public void OnLoginClick()
    {
        alertTextLogIn.text = "Signing in...";
       // ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    public void OnCreateClick()
    {
        alertTextSignUp.text = "Creating account...";
       // ActivateButtons(false);

        StartCoroutine(TryCreate());
    }
    #endregion




  #region private methods
    void CreateOrJoinRandomRoom()
    {
        string randomRoom = "Room " + Random.Range(0, 5000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 1;
        PhotonNetwork.CreateRoom(randomRoom, roomOptions);
    }
    


    
    private IEnumerator TryLogin()
    {
        string gameid = usernameInputFieldLogin.text;
        string password = passwordInputFieldLogin.text;

        if (gameid.Length > 24)
        {
            alertTextLogIn.text = "Invalid username";
           // ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertTextLogIn.text = "Invalid credentials";
           // ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rGameid", gameid);
        form.AddField("rPassword", password);

       // UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);

        Debug.Log("username "+gameid+" pass "+password);
       UnityWebRequest request = UnityWebRequest.Get($"{loginEndpoint}?rGameid={gameid}&rPassword={password}");

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if (response.code == 0) // login success?
            {
               // ActivateButtons(false);
                alertTextLogIn.text = "Welcome " + ((response.data.adminFlag == 1) ? " Admin" : "");


                OnClickEnterGameJoinRandomRoom();
              
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertTextLogIn.text = "Invalid credentials";
                       // ActivateButtons(true);
                        break;
                    default:
                        alertTextLogIn.text = "Corruption detected";
                      //  ActivateButtons(false);
                        break;
                }
            }
        }
        else
        {
            alertTextLogIn.text = "Error connecting to the server...";
            //ActivateButtons(true);
        }


        yield return null;
    }

    private IEnumerator TryCreate()
    {
        string username = usernameInputFieldSignUp.text;
        string password = passwordInputFieldSignUp.text;
        string gameID  = gameIDInputFieldSignUp.text;


        Debug.Log("Sign Up info: "+username+" "+password+" "+gameID);

        if (username.Length < 3 || username.Length > 24)
        {
            alertTextSignUp.text = "Invalid username";
           // ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertTextSignUp.text = "Invalid credentials1";
          //  ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

                                                                                                                                                
       // UnityWebRequest request = UnityWebRequest.Post(createEndpoint, form);
       UnityWebRequest request = UnityWebRequest.Get($"{createEndpoint}?rUsername={username}&rPassword={password}&rGameid={gameID}");
       Debug.Log("Form: "+ form+" web request "+request);

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 30.0f)
            {           
                break;
            }
         yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            CreateResponse response = JsonUtility.FromJson<CreateResponse>(request.downloadHandler.text);

            if (response.code == 0) 
            {
                alertTextSignUp.text = "Account has been created!";
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertTextSignUp.text = "Invalid credentials2";
                        break;
                    case 2:
                        alertTextSignUp.text = "Username is already taken";
                        break;
                    case 3:
                        alertTextSignUp.text = "Password is unsafe";
                        break;
                    default:
                        alertTextSignUp.text = "Corruption detected";
                        break;

                }
            }
        }
        else
        {
            alertTextSignUp.text = "Error connecting to the server...";
        }

       // ActivateButtons(true);

        yield return null;
    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        createButton.interactable = toggle;
    }
        #endregion


        
    #region PUN call backs
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateOrJoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " Joined " + PhotonNetwork.CurrentRoom.Name); ///this will show who joined which room

        PhotonNetwork.LoadLevel("RoomScene");/// this will load another scene
    }
    #endregion

}
