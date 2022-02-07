using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject HousePrefab;
    [SerializeField]
    GameObject button;
    public Text RoomNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab != null)
        {
            if (PhotonNetwork.IsConnected)
            {
                int random = Random.Range(2, 5);
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(random, 0f, random), Quaternion.identity,0);
            }
        }
        
    }
    void Update()
    {
        RoomNumber.text = PhotonNetwork.LocalPlayer.NickName + "'s ROOM";
    }
    public void onClickBuild()
    {
        int rand = Random.Range(2,1);
        PhotonNetwork.Instantiate(HousePrefab.name, new Vector3(rand,0f,rand), Quaternion.identity,0);
        Destroy(button);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " Joined " + PhotonNetwork.CurrentRoom.Name);
    }
    public void Room_Status()
    {
        
    }


}
