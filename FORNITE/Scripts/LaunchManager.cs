using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviourPunCallbacks
{

    public GameObject LogInorSignUpPanel;
    public GameObject SignUpPanel;
    public GameObject LogInPanel;
    public GameObject connectionPanel;

    public Text connectionStatusText;
    // Start is called before the first frame update
    void Start()
    {
        LogInorSignUpPanel.SetActive(true);
        SignUpPanel.SetActive(false);
        LogInPanel.SetActive(false);
        connectionPanel.SetActive(false);
        if (!PhotonNetwork.IsConnected)
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
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
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
    public void onClickSignUp() //When Sign Up button is clicked
    {
        LogInPanel.SetActive(false);
        LogInorSignUpPanel.SetActive(false);
        SignUpPanel.SetActive(true);
        connectionPanel.SetActive(false);
    }
    public void onClickLogIN() //When LogIn or create game account button is clicked
    {
        LogInPanel.SetActive(true);
        LogInorSignUpPanel.SetActive(false);
        SignUpPanel.SetActive(false);
        connectionPanel.SetActive(false);
    }
    public void OnClickBackButton()
    {
        LogInPanel.SetActive(false);
        LogInorSignUpPanel.SetActive(true);
        SignUpPanel.SetActive(false);
        connectionPanel.SetActive(false);
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
