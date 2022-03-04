//Class written by: Dev Patel

using System.Collections;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager _instance;

    public int[] Coins { get; set; }

    //Different coin generation times for singleplayer (SP) and multiplayer (MP)
    [SerializeField]
    private float SP_TimeBetweenGeneration;
    [SerializeField]
    private float MP_TimeBetweenGeneration;
    [SerializeField]
    private float AI_TimeBetweenGeneration;

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

    public void Setup()
    {
        Coins = new int[2];
    }

    public void BeginGeneration()
    {
        //player 1 (index 0) generates coin at SP_TimeBetweenGeneration rate, player 2/AI (index 1) generates coin at AI_TimeBetweenGeneration rate
        if (GameplayManager._instance.SoloMode)
        {
            StartCoroutine(GenerateCoins(SP_TimeBetweenGeneration, 0));
            StartCoroutine(GenerateCoins(AI_TimeBetweenGeneration, 1));
        }
        //both players (humans/local multiplayer) generate coins at MP_TimeBetweenGeneration rate
        else
        {
            StartCoroutine(GenerateCoins(MP_TimeBetweenGeneration, 0));
            StartCoroutine(GenerateCoins(MP_TimeBetweenGeneration, 1));
        }
    }

    //in solo mode, coins are only generate for player 1 (0 index of the array)
    //in multiplayer, coins are generated for both players
    private IEnumerator GenerateCoins(float time, int player)
    {
        Coins[player] += 1;
        CanvasManager._instance.UpdateDisplayedData();
        yield return new WaitForSeconds(MP_TimeBetweenGeneration);
        StartCoroutine(GenerateCoins(time, player));
    }
}
