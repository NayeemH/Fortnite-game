using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Portal : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        if(other.gameObject.tag == "Player")
        {
            PhotonNetwork.LoadLevel("OpenWorldScene");
            Destroy(gameObject);
            PhotonNetwork.JoinRandomRoom();
        }
    }


    public override void OnConnected()
    {
        base.OnConnected();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }
    void CreateOrJoinRandomRoom()
    {
        string randomRoom = "Room " + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(randomRoom, roomOptions);
    }




    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateOrJoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
