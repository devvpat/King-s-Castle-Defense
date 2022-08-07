//Class written by Dev Patel
//Note: this was made partially following the PUN 2 tutorial - https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/intro
//As such, a lot of the code is likely copied (including access modifiers) as explained in the tutorial/possibly changed a bit to fit the scope of this game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//will listen for key presses and also is used for photon multiplayer
public class PlayerChar : MonoBehaviourPun
{
    private int PlayerNum;

    public void SetPlayerNum(int n) { PlayerNum = n; }

    // Update is called once per frame
    void Update()
    {
        //in online mode, only allow for user inputs if it is your character
        if (GameplayManager._instance.OnlineMode && PhotonNetwork.IsConnected && !photonView.IsMine) return;
        if (GameStateManager.state == GameStateManager.GameState.Playing)
        {
            //spawn castle/knight characters
            if (PlayerNum == 1)
            {
                if (Input.GetButtonDown("SpawnCastle1"))
                {
                    CharacterManager._instance.SpawnCharacter("C1", 1);
                }
                if (Input.GetButtonDown("SpawnCastle2"))
                {
                    CharacterManager._instance.SpawnCharacter("C2", 1);
                }
                if (Input.GetButtonDown("SpawnCastle3"))
                {
                    CharacterManager._instance.SpawnCharacter("C3", 1);
                }
                if (Input.GetButtonDown("SpawnCastle4"))
                {
                    CharacterManager._instance.SpawnCharacter("C4", 1);
                }
                if (Input.GetButtonDown("SpawnCastle5"))
                {
                    CharacterManager._instance.SpawnCharacter("C5", 1);
                }
            }
            //spawn pirates
            if (PlayerNum == 2)
            {
                if (Input.GetButtonDown("SpawnPirate1"))
                {
                    CharacterManager._instance.SpawnCharacter("P1", 2);
                }
                if (Input.GetButtonDown("SpawnPirate2"))
                {
                    CharacterManager._instance.SpawnCharacter("P2", 2);
                }
                if (Input.GetButtonDown("SpawnPirate3"))
                {
                    CharacterManager._instance.SpawnCharacter("P3", 2);
                }
                if (Input.GetButtonDown("SpawnPirate4"))
                {
                    CharacterManager._instance.SpawnCharacter("P4", 2);
                }
                if (Input.GetButtonDown("SpawnPirate5"))
                {
                    CharacterManager._instance.SpawnCharacter("P5", 2);
                }
            }
        }
    }
}
