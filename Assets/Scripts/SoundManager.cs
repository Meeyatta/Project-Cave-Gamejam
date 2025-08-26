using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound { Test, }

[System.Serializable]
public class SoundEffect
{
    public string Name;
    public Sound Sound;
    public AudioClip Audio;

    public SoundEffect(string n, Sound s, AudioClip a)
    {
        Name = n; Sound = s; Audio = a;
    }
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance;

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

    [SerializeField]
    public List<SoundEffect> Sounds;
    public AudioSource SoundObject;

    AudioClip GetClip(Sound s)
    {
        foreach (var v in Sounds)
        {
            if (v.Sound == s) { return v.Audio; }
        }

        Debug.Log("Didnt find sound");
        return null;
    }

    public void PlaySound(Sound sound, float volume, Transform transform)
    {
        AudioSource audioSource = Instantiate(SoundObject, transform.position, Quaternion.identity);

        AudioClip c = GetClip(sound); if (c == null) { return; }
        audioSource.clip = c;
        audioSource.volume = volume;
        audioSource.Play();

        float l = audioSource.clip.length;
        Destroy(audioSource.gameObject, l); 
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
