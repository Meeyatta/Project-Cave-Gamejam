using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public float DroningRange;
    public float MinRange;

    bool SetToNone;

    public PlayerMovement pm;
    public TorchManager tm;

    public List<GameObject> Spawners = new List<GameObject>();

    public AudioSource Music;
    public GameObject Droning;
    public GameObject Enemies;

    public void BurntDownFinalGate()
    {
        Music.enabled = false;
        foreach (var v in Spawners) { v.SetActive(false); }

        foreach (var v in FindObjectsOfType<EnemyAI>()) { v.gameObject.SetActive(false); }
    }

    public void StartDroning()
    {
        Droning.SetActive(true);
        tm.Power_Add(9990, 3);
        SoundManager.Instance.PlaySound(Sound.Ignite, 1, transform);
        SetToNone = true;
    }
    bool isEnding = false;
    public IEnumerator End()
    {
        pm.CanMove = false;

        Enemies.SetActive(true);

        yield return new WaitForSeconds(1f);
        SetToNone = false;
        tm.Cur_Power = tm.Max_Power;


        yield return new WaitForSeconds(3f);

        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (SetToNone) tm.Cur_Power = 0;

        if (Vector3.Distance(transform.position, tm.transform.position) < DroningRange && !Droning.activeSelf)
        {
            StartDroning();
        }

        if (Vector3.Distance(transform.position, tm.transform.position) < MinRange && !isEnding)
        {
            StartCoroutine(End());
        }
    }
}
