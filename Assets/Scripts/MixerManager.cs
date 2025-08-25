using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    #region Singleton
    public static MixerManager Instance;

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public AudioMixer Mixer;

    const string master_volume = "master_volume";
    const string sfx_volume = "sfx_volume";
    const string music_volume = "music_volume";

    public void SetVolume_master(float v)
    {
        Mixer.SetFloat(master_volume, Mathf.Log10(v) * 20f);
    }
    public void SetVolume_sfx(float v)
    {
        Mixer.SetFloat(sfx_volume, Mathf.Log10(v) * 20f);
    }
    public void SetVolume_music(float v)
    {
        Mixer.SetFloat(music_volume, Mathf.Log10(v) * 20f);
    }

    void Start()
    {
        
    }

    private void Awake()
    {
        Singleton();
    }

    void Update()
    {
        
    }
}
