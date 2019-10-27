using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioController : MonoBehaviour {

    public AudioPlayer player;
    public Slider musicSlider;
    public Slider soundSlider;
    [SerializeField] GameObject soundImg, musicImg, noMusicImg, noSoundImg;
    [SerializeField] int musicPlayTime;
    private float nextTime = 0;
    //TODO: Update slider dynamically and bound slider's value to audiosource value;
    public void ChangeMusicVolume(float value)
    {
        player.ChangeMusicVolume(value);
    }
    public void ChangeSoundVolume(float value)
    {
        player.ChangeSoundVolume(value);
    }
    public void ActivateMusic(bool state)
    {
        if (state)
        {
            player.MuteMusic(!state);
            musicSlider.value = player.GetDefaultVolume();
        }
        else
        {
            player.MuteMusic(!state);
            musicSlider.value = 0;
        }
        //TODO: update slider
    }
    public void ActivateSounds(bool state)
    {
        if (state)
        {
            player.MuteSounds(!state);
            soundSlider.value = player.GetDefaultVolume();
            
        }
        else
        {
            player.MuteSounds(!state);
            soundSlider.value = 0;
        }
        
    }
    private void SwapImages(GameObject startImg, GameObject endImg)
    {
        startImg.SetActive(false);
        endImg.SetActive(true);
    }
    public void MakeClickSound()
    {
        player.MakeClickSound();
    }
    public void MakeSound(AudioClip clip)
    {
        player.MakeSound(clip);
    }
    public void StartAmbienceSound(AudioClip clip)
    {
        player.MakeAmbience(clip);
    }
    private void Start()
    {
        player.SetDefaultVolume();
        musicSlider.value = player.GetDefaultVolume();
        musicSlider.onValueChanged.AddListener(delegate { SwapImages(noMusicImg, musicImg); });
        soundSlider.value = player.GetDefaultVolume();
        soundSlider.onValueChanged.AddListener(delegate { SwapImages(noSoundImg, soundImg); });
        player.PlayRandomMusic();
        nextTime = Time.time + musicPlayTime;
    }
   
    private void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime = Time.time + musicPlayTime;
            StartCoroutine(player.ShiftBetweenMusic());
        }
        
    }
  

}
