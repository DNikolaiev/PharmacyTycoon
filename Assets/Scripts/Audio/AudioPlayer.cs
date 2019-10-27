using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioPlayer  {

    [SerializeField] AudioSource clickSource, soundSource, musicSource, ambienceSource;
    [SerializeField] AudioClip[] clickClips;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] float minVolume, maxVolume, standardVolume;
    public void MakeClickSound()
    {
        clickSource.PlayOneShot(clickClips[Random.Range(0, clickClips.Length)], Random.Range(minVolume, maxVolume));
    }
    public void MakeClickSound(AudioClip clip)
    {
        if(clip!=null)
        clickSource.PlayOneShot(clip, Random.Range(minVolume, maxVolume));
    }
    public void MakeSound(AudioClip clip)
    {
        if(clip!=null)
        soundSource.PlayOneShot(clip, Random.Range(minVolume, maxVolume));
    }
    public void MakeAmbience(AudioClip clip)
    {
        if (clip == null) return;
        ambienceSource.clip = clip;
        ambienceSource.loop = true;
       
        ambienceSource.Play();
    }
    public void StopAmbience()
    {
        ambienceSource.clip = null;
        ambienceSource.Stop();
    }
    public IEnumerator ShiftBetweenMusic()
    {
        if (musicClips.Length == 0) yield break;
        float volume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
        if (musicSource.volume == 0)
        {
            PlayRandomMusic();
            while (musicSource.volume < volume)
            {
                musicSource.volume += 0.1f;
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }
    public void SetDefaultVolume()
    {
        SetDefaultMusicVolume();
        SetDefaultSoundVolume();
        
    }
    public void SetDefaultMusicVolume()
    {
        musicSource.volume = standardVolume;
    }
    public void SetDefaultSoundVolume()
    {
        soundSource.volume = standardVolume;
        clickSource.volume = standardVolume;
        ambienceSource.volume = standardVolume;
    }
    public float GetDefaultVolume()
    {
        return standardVolume;
    }
    public void PlayRandomMusic()
    {
        musicSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        musicSource.Play();
    }
    public void PauseMusic(bool state)
    {
        if (state)
            musicSource.Pause();
        else musicSource.UnPause();
    }
    public void MuteMusic(bool state)
    {
        if (state)
            musicSource.volume = 0;
        else musicSource.volume = standardVolume;
    }
    public void MuteSounds(bool state)
    {
        if (state)
        {
            soundSource.volume = 0;
            clickSource.volume = 0;
            ambienceSource.volume = 0;
        }
        else
        {
            soundSource.volume = standardVolume;
            clickSource.volume = standardVolume;
            ambienceSource.volume = standardVolume;
        }
    }
    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }
    public void ChangeSoundVolume(float value)
    {
        soundSource.volume = value;
        clickSource.volume = value;
        ambienceSource.volume = value;
    }
}
