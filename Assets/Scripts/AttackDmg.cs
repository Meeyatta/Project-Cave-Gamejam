using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDmg : MonoBehaviour
{
    PlayerAttack pa;

    private void Start()
    {
        pa = transform.parent.GetComponent<PlayerAttack>();
    }

    public void GatherTargets()
    {
        //Stopped working here
    }



    public void Attack()
    {
        foreach (var v in pa.Trigger.CurTargets)
        {
            v.Hp.Take_Dmg(pa.Damage);
        }
        pa.Attack_Next = Time.time + pa.Attack_Cooldown;
    }
}
