using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Data", fileName = "Game Data")]
public class GameData : ScriptableObject
{
    public int SP_Score;
    public int SP_Coins;

    public int[] MP_Score;
    public int[] MP_Coins;


    public void ResetData()
    {
        SP_Coins = 0;
        SP_Score = 0;
        for (int i = 0; i < MP_Coins.Length; i++) MP_Coins[i] = 0;
        for (int i = 0; i < MP_Score.Length; i++) MP_Score[i] = 0;
    }
}
