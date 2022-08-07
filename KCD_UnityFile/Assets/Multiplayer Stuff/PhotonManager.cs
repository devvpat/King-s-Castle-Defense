//Class written by Dev Patel
//Note: a part of this was made following the PUN 2 tutorial - https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/intro
//As such, a lot of the code is likely copied (including access modifiers) as explained in the tutorial/possibly changed a bit to fit the scope of this game

//Networked multiplayer code - not used

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void NewOnlineGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("PhotonNetwork : Loading Level");
        GameStateManager.state = GameStateManager.GameState.Playing;
        PlayerPrefs.SetInt("SoloMode", 0);
        PlayerPrefs.SetInt("Online", 1);
        PhotonNetwork.LoadLevel(GameStateManager.instance.GetGameSceneName()); //use photon for online multiplayer
    }

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            NewOnlineGame();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            NewOnlineGame();
        }
    }

    #endregion
}
