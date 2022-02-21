//Class written by: Dev Patel

using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void QuitApp()
    {
        GameStateManager.instance.QuitGame();
    }
}
