using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
    public float Max_Power;
    public float Drain_Amount;
    public float Drain_Cooldown;


    [Header("---")]
    public Light Torch_Light;
    public float Cur_Power;

    const float MaxAngle = 90;

    void Start()
    {
        Cur_Power = Max_Power;

        StartCoroutine(PassiveDrain());
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
        while (elapsed < t)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            elapsed += Time.deltaTime;
            Cur_Power = Mathf.Clamp(Cur_Power + (p / t) * Time.deltaTime, -1, Max_Power);
        }
    }

    void Power_Zero()
    {
        Debug.Log("Player ran out of power");
    }

    IEnumerator PassiveDrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(Drain_Cooldown * Time.fixedDeltaTime);
            Cur_Power = Mathf.Clamp(Cur_Power - Drain_Amount, -1, Max_Power);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Cur_Power <= 0) { Power_Zero(); }

        //90 degrees - full power, 0 degrees - fully drained
        Torch_Light.spotAngle = Cur_Power / Max_Power * MaxAngle;

        if (Input.GetKeyDown("p"))
        {
            StartCoroutine(Power_Add(25, 1f));
        }
    }
}
