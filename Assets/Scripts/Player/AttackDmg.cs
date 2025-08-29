using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDmg : MonoBehaviour
{
    public List<Health> damaged = new List<Health>();
    PlayerAttack pa;
    public Animator Anim;
    const string AttackLayer = "AttackLayer";
    int ind;
    private void Start()
    {
        pa = transform.parent.GetComponent<PlayerAttack>();
        ind = Anim.GetLayerIndex(AttackLayer);
    }

    //Basically when we attack, the script will check what we CAN attack multiple times
    public void Attack_beforehands()
    {
        Anim.SetLayerWeight(ind, 1);
        foreach (var v in pa.Trigger.CurTargets)
        {
            if (!damaged.Contains(v.Hp)) { v.Hp.Take_Dmg(pa.Damage); damaged.Add(v.Hp); }
        }
    }

    public void Attack_last()
    {
        foreach (var v in pa.Trigger.CurTargets)
        {
            if (!damaged.Contains(v.Hp)) { v.Hp.Take_Dmg(pa.Damage); damaged.Add(v.Hp); }
        }
        damaged.Clear();
        Anim.SetLayerWeight(ind, 0);
    }
}
