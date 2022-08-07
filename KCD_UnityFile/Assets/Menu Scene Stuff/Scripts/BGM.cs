//Tien-Yi Lee (thanks to Hooson's tutorial https://www.youtube.com/watch?v=yWCHaTwVblk&ab_channel=Hooson)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
