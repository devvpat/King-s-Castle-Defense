//Class written by: Dev Patel
//Note: this was made following the PUN 2 tutorial - https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/intro
//As such, a lot of the code is likely copied (including access modifiers) as explained in the tutorial/possibly changed a bit to fit the scope of this game

//Networked multiplayer code - not used

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializbale Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte MaxPlayersPerRoom = 2;
    #endregion

    #region Private Fields

    private string GameVersion = "1";

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject ControlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject ProgressLabel;

    private bool IsConnecting;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    #endregion

    #region Public Methods

    public void Connect()
    {
        //game crashes if trying to connect while not on master server (while on game server)
        if (PhotonNetwork.Server == ServerConnection.MasterServer)
        {
            ProgressLabel.SetActive(true);
            ControlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                IsConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GameVersion;
            }
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        if (IsConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            IsConnecting = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("We have two players in room and now load the game");
            //PhotonNetwork.LoadLevel(GameStateManager.instance.GetGameSceneName());
            PhotonManager._instance.NewOnlineGame();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ProgressLabel.SetActive(false);
        ControlPanel.SetActive(true);
        IsConnecting = false;
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    #endregion
}
