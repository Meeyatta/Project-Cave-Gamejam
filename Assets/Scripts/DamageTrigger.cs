using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public List<string> Tags_include = new List<string>();
    public List<string> Tags_exclude = new List<string>();

    public List<Hp_Gobj> CurTargets = new List<Hp_Gobj>();

    [System.Serializable]
    public class Hp_Gobj
    {
        public Health Hp;
        public GameObject Object;

        public Hp_Gobj(Health h, GameObject g)
        {
            Hp = h; Object = g;   
        }
    }

    Hp_Gobj GetHp(GameObject o)
    {
        foreach (var v in CurTargets)
        {
            if (v.Object == o) return v;
        }

        return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Tags_include.Contains(other.gameObject.tag) || Tags_exclude.Contains(other.gameObject.tag)) return;
    }
    private void OnTriggerExit(Collider other)
    {
        Hp_Gobj hg = GetHp(other.gameObject);
        if (hg != null && CurTargets.Contains(hg)) { CurTargets.Remove(hg); }
    }
}
