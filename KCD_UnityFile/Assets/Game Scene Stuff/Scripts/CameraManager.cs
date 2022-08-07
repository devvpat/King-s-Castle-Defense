//Class written by: Dev Patel

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager _instance;

    [SerializeField]
    private float CameraX;
    [SerializeField]
    private float CameraZ;
    private float CameraY;
    [SerializeField]
    private float SP_CameraY;
    [SerializeField]
    private float MP_CameraY;

    [SerializeField]
    private Camera cam;

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

    //Sets the camera's position depending on if the game is being played solo or not
    public void Setup()
    {
        if (GameplayManager._instance.SoloMode) CameraY = SP_CameraY;
        else CameraY = MP_CameraY;
        cam.transform.position = new Vector3(CameraX, CameraY, CameraZ);
    }

}
