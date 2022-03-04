//Class written by: Dev Patel
//Note: this was made following the PUN 2 tutorial - https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/intro
//As such, a lot of the code is likely copied as explained in the tutorial/possibly changed a bit to fit the scope of this game

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants

    private const string PlayerNamePrefKey = "PlayerName";

    private int MaxNameLength = 10;

    private TMP_InputField Input;

    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        string defaultName = "Player";
        Input = this.GetComponent<TMP_InputField>();
        if (Input != null)
        {
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                Input.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Player Name is null or empty");
            return;
        }
        if (name.Length > MaxNameLength)
        {
            name = name.Substring(0, MaxNameLength);
            Input.text = name;
        }
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(PlayerNamePrefKey, name);
    }

    #endregion
}
