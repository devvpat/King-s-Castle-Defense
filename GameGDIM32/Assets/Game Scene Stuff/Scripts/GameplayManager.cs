using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager _instance;

    [SerializeField]
    private GameData GD;

    [SerializeField]
    private GameObject KingPrefab;
    public bool SoloMode;

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

    // Start is called before the first frame update
    void Start()
    {
        GD.ResetData();
        CanvasManager._instance.UpdateData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetSoloMode()
    {
        return SoloMode;
    }
}
