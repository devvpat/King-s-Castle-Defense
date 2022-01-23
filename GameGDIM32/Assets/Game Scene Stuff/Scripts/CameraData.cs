using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Camera Data", fileName = "Camera Data")]
public class CameraData : ScriptableObject
{
    //Stores Camera's Y position when game is played solo
    public float SoloCameraY;
    //Stores Camera's Y position when game is played with two players
    public float MPCameraY;
    //Stores if the camera should use Y position used in solo mode
    public bool SoloMode;
}
