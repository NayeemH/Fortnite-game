using UnityEngine;

public class PlayerData 
{
    #region Public Variables


    public string playerName;
    public string playerID;
    public string password;


    #endregion

    #region Public Methods

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json)
    {
        return JsonUtility.FromJson<PlayerData>(json);
    }



    #endregion

}
