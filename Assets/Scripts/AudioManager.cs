using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance;
    [Range(0f,1f)]
    public float MusicVolume;
    public AudioSource candySFX;
    private AudioSource audioMusic;  
    public Slider musicSlider;
    public Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        audioMusic = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        audioMusic.volume = musicSlider.value;
        candySFX.volume = sfxSlider.value;
    }
}
