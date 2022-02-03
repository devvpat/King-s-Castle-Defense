using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{

    float scrollSpeed = -80f;
    Vector2 startPos;

    [SerializeField]
    public int length;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //so the script will run after the first game
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, length);
        transform.position = startPos + Vector2.left * newPos;
    }
}
