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
        bool solo = GameplayManager._instance.SoloMode;
        StartCoroutine(GenerateCoins(solo));
    }

    //in solo mode, coins are only generate for player 1 (0 index of the array)
    //in multiplayer, coins are generated for both players
    private IEnumerator GenerateCoins(bool solo)
    {
        if (solo)
        {
            Coins[0] += 1;
            CanvasManager._instance.UpdateDisplayedData();
            yield return new WaitForSeconds(SP_TimeBetweenGeneration);
            StartCoroutine(GenerateCoins(solo));
        }
        else
        {
            for (int i = 0; i < Coins.Length; i++)
            {
                Coins[i] += 1;
            }
            CanvasManager._instance.UpdateDisplayedData();
            yield return new WaitForSeconds(MP_TimeBetweenGeneration);
            StartCoroutine(GenerateCoins(solo));
        }
    }
}
