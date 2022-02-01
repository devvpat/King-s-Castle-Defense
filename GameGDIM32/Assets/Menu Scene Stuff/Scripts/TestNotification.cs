//Tien-Yi (Just for testing) delete if needed
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNotification : MonoBehaviour
{

    //just for testing the notifications!
    //just for testing the notifications!
    //just for testing the notifications!


    private void Start()
    {
        Debug.Log("test");
        NotificationManager.Instance.SetNewNotification("test test");
        
    }

    private void Update()
    {
        if(Time.time > 5f)
        {
            NotificationManager.Instance.SetNewNotification("123");
            Destroy(gameObject);
        }
    }
}
