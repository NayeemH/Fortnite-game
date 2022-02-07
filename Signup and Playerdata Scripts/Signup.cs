using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Signup : MonoBehaviour
{

    private PlayerData playerData;

    [Header("Signup_ UI_Panel")]
    public InputField playerName;
    public InputField playerId;
    public InputField password;








    // Start is called before the first frame update
    void Start()
    {
        playerData = new PlayerData();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

    }

    public void OnLoginClicked()
    {
        string currentPlayerId = playerId.text;
        string currentPlayerPassword = password.text;
        StartCoroutine(Download(currentPlayerId, result =>      ///Downloading player data using id
        {
            Debug.Log(result);
            if (result != null)
            {
                playerData.playerID = result.playerID;
                playerData.password = result.password;
                playerData.playerName = result.playerName;
            }

        }));

        if(currentPlayerId == playerData.playerID)
        {
            if(currentPlayerPassword == playerData.password)
            {
                Debug.Log("Login Successful!"); /// Load Scene

            }
            else
            {
                Debug.Log("Password is Incorrect!");
            }
        }
        else
        {
            Debug.Log("Id is Incorrect!");
        }
    }

    public void OnCreateGameAccountClicked()
    {
        playerData.playerID = playerId.text;
        playerData.playerName = playerName.text;
        playerData.password = password.text;

        StartCoroutine(Upload(playerData.Stringify(), result =>
         {
             Debug.Log(result);                         ///Uploading PlayerData to DB
         }));

    }

    IEnumerator Download(string id, System.Action<PlayerData> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("" + id))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError  || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(PlayerData.Parse(request.downloadHandler.text));
                }
            }
        }

       
    }

    IEnumerator Upload(string profile, System.Action<bool> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest("", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(false);
                }
            }
            else
            {
                if(callback!=null)
                {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
        }
    }

}
