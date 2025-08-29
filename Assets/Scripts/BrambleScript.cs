using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrambleScript : MonoBehaviour
{
    public bool IsLit;
    public List<ParticleSystem> Particles;

    public void LightUp()
    {
        IsLit = true;
        foreach (var p in Particles) { p.Play(); }
    }

    void Start()
    {
        foreach (var p in Particles) { p.Stop(); }
    }

    void Update()
    {

    }
}
