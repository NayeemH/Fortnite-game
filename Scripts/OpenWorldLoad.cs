using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OpenWorldLoad : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject playerpref;
    private void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            if(playerpref != null)
            {
                int rand = Random.Range(270,500);
                PhotonNetwork.Instantiate(playerpref.name, new Vector3(rand, 0f, rand), Quaternion.identity);
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
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
