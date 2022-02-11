using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

    [SerializeField]
    TextMeshProUGUI playerNametext;
    private void Start()
    {
        setPlayerUI();
    }
    void setPlayerUI()
    {
        if(playerNametext!=null)
        playerNametext.text = photonView.Owner.NickName;
    }