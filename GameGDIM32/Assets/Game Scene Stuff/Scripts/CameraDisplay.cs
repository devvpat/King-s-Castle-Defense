using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplay : MonoBehaviour
{
    [SerializeField]
    private CameraData CD;
    [SerializeField]
    private float CameraX;
    [SerializeField]
    private float CameraZ;

    private float CameraY;

    //Sets the camera's position depending on if the game is being played solo or not
    void Start()
    {
        if (CD.SoloMode) CameraY = CD.SoloCameraY;
        else CameraY = CD.MPCameraY;
        transform.position = new Vector3(CameraX, CameraY, CameraZ);
    }

}
