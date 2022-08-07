//Class written by: Dev Patel

using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager _instance;

    public int[] Scores { get; private set; }

    //Wait time between score increasing
    [SerializeField]
    private float WaitTime;

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
        Scores = new int[2];
    }

    public float GetWaitTime()
    {
        return WaitTime;
    }

    public void StartScoreCount()
    {
        bool solo = GameplayManager._instance.SoloMode;
        StartCoroutine(IncreaseScore(solo));    
    }

    //Solomode increments on player 1's score, multiplayer mode increments both players scores (stored in Scores array)
    private IEnumerator IncreaseScore(bool solo)
    {
        if (solo)
        {
            Scores[0] += 1;
            CanvasManager._instance.UpdateDisplayedData();
            yield return new WaitForSeconds(WaitTime);
            StartCoroutine(IncreaseScore(solo));
        }
        else
        {
            for (int i = 0; i < Scores.Length; i++)
            {
                Scores[i] += 1;
            }
            CanvasManager._instance.UpdateDisplayedData();
            yield return new WaitForSeconds(WaitTime);
            StartCoroutine(IncreaseScore(solo));
        }
    }
}
