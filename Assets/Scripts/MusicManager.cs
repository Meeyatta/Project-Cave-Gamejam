using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip Morning;
    public AudioClip Evening;
    public AudioClip Night;
    public AudioSource Source;
    #region Singleton
    public static MusicManager Instance;

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
    private void Awake()
    {
        Singleton();
    }

    public void Play(int i)
    {
        switch (i)
        {
            case 1:
                Source.clip = Morning;
                Source.Play();
                break;
            case 2:
                Source.clip = Evening;
                Source.Play();
                break;
            default:
                Source.clip = Night;
                Source.Play();
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
