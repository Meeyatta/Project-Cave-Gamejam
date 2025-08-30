using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
    public float Max_Power_Base;
    public float Min_Power;
    public float Base_Drain;
    public float Drain_Cooldown;

    public List<ParticleSystem> Effects_On;
    public List<GameObject> Effects_Off;

    [Header("---")]
    public float Max_Power;
    public float Drain_Amount;
    public Light Torch_Light;
    public float Cur_Power;

    const float MaxAngle = 90;

    BuffsManager bm;
    void Start()
    {
        Cur_Power = Max_Power;

        bm = GetComponent<BuffsManager>();
        StartCoroutine(PassiveDrain());
    }

    public void BuffUpdates()
    {
        //1 a +x health, but +y passive torch drain
        Drain_Amount = Base_Drain + bm.a_drain * bm.GetBuffAmount(1);

        //                     5 e +x damage dealt, -x maximum torch power
        Max_Power = Max_Power_Base + bm.e_power * bm.GetBuffAmount(5);
    }

    public IEnumerator Power_Drain(float p, float t)
    {
        float pt = p / t; float elapsed = t;


        while (elapsed < t)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            elapsed += Time.deltaTime;
            Cur_Power = Mathf.Clamp(Cur_Power - (p / t) * Time.deltaTime, -1, Max_Power);
        }
    }
    public IEnumerator Power_Add(float p, float t)
    {
        float elapsed = 0;

        foreach (var v in Effects_Off) { v.SetActive(false); }
        foreach (var v in Effects_On) { v.Play(); }

        while (elapsed < t)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            elapsed += Time.deltaTime;
            Cur_Power = Mathf.Clamp(Cur_Power + (p / t) * Time.deltaTime, Min_Power, Max_Power);
        }

        foreach (var v in Effects_Off) { v.SetActive(false); }
        foreach (var v in Effects_On) { v.Play(); }
    }

    void Power_Zero()
    {
        Debug.Log("Player ran out of power");
        foreach (var v in Effects_Off) { v.SetActive(true); }
        foreach (var v in Effects_On) { v.Stop(); }
    }

    IEnumerator PassiveDrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(Drain_Cooldown * Time.fixedDeltaTime);
            Cur_Power = Mathf.Clamp(Cur_Power - Drain_Amount, Min_Power, Max_Power);
        }

    }
    private void FixedUpdate()
    {
        BuffUpdates();
    }
    void Update()
    {
        if (Cur_Power <= Min_Power + Drain_Amount) { Power_Zero(); }

        //90 degrees - full power, 0 degrees - fully drained
        Torch_Light.spotAngle = Cur_Power / Max_Power * MaxAngle;

        //if (Input.GetKeyDown("p"))
        //{
        //    StartCoroutine(Power_Add(25, 1f));
        //}
    }
}
