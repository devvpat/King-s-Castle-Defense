using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private bool PlaySolo;

    [SerializeField]
    private GameObject King;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(King, new Vector3(18, 1, -5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
