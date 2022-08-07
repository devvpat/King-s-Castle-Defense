//Tien-Yi Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI notificationText;

    [SerializeField] private float fadeTime;

    private IEnumerator notificationCoroutine;

    private static NotificationManager instance;

    public static NotificationManager Instance
    {
        get //check and try to find reference, if not then create
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<NotificationManager>();

            if (instance != null)
            {
                return instance;
            }

            CreateNewInstance();

            return instance;
        }
    }

    public static NotificationManager CreateNewInstance() //create the type we want
    {
        NotificationManager notificationManagerPrefab = Resources.Load<NotificationManager>("NotificationManager");
        instance = Instantiate(notificationManagerPrefab);

        return instance;
    }

    private void Awake() //make sure this is the only one
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetNewNotification(string message) //pass in the string, so we can use it on the other script
    {
        if(notificationCoroutine != null)         //if notification is currently fading out, 
        {                                         // and stop it
            StopCoroutine(notificationCoroutine); 
        }
        notificationCoroutine = FadeOutNotification(message); //then make the notification we passed in to fade out 
        StartCoroutine(notificationCoroutine);                //and start it
    }

    private IEnumerator FadeOutNotification(string message) //fade out animation
    {
        notificationText.text = message; //replacing text we want to show
        float t = 0;
        while(t < fadeTime) 
        {
            t += Time.unscaledDeltaTime; //use t+= Time.DeltaTime; if we want the notification to pause too
            notificationText.color = new Color(
                notificationText.color.r, 
                notificationText.color.g, 
                notificationText.color.b, 
                Mathf.Lerp(1f, 0f, t/fadeTime)); //just to change the transparency of the text

            yield return null; //need this if it takes time over frame (color change)
        }
    }
}
