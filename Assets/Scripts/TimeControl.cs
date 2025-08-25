using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    [Header("During the day control")]
    public float TimeSpeed;
    public float Delay;

    public float End_Morning;
    public float End_Evening;
    public float End_Night;

    [Header("---")]
    public float CurTime;

    public bool IsMorning;
    public bool IsEvening;
    public bool IsNight;


    [Header("Day controll")]
    public int CurDay;

    public static TimeControl Instance;

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

    void Start()
    {
        StartCoroutine(TimePassage());
    }
    IEnumerator TimePassage()
    {
        while (true)
        {
            yield return new WaitForSeconds(Delay * Time.fixedDeltaTime);

            CurTime += TimeSpeed;

            if (CurTime <= End_Morning) { Morning(); }
            if (CurTime > End_Morning && CurTime <= End_Evening) { Evening(); }
            if (CurTime > End_Evening && CurTime <= End_Night) { Night(); }
            if (CurTime > End_Night) { CurDay++; CurTime = 0; }
        }

    }

    void Morning()
    {
        IsMorning = true; IsEvening = false; IsNight = false;
    }
    void Evening()
    {
        IsMorning = false; IsEvening = true; IsNight = false;
    }
    void Night()
    {
        IsMorning = false; IsEvening = false; IsNight = true;
    }

    private void Awake()
    {
        Singleton();
    }
    void Update()
    {
        
    }
}
