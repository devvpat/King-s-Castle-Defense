using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplay : MonoBehaviour
{
    [SerializeField]
    private float CameraX;
    [SerializeField]
    private float CameraZ;
    private float CameraY;
    [SerializeField]
    private float SP_CameraY;
    [SerializeField]
    private float MP_CameraY;

    //Sets the camera's position depending on if the game is being played solo or not
    void Start()
    {
        if (GameplayManager._instance.GetSoloMode()) CameraY = SP_CameraY;
        else CameraY = MP_CameraY;
        transform.position = new Vector3(CameraX, CameraY, CameraZ);
    }

}
