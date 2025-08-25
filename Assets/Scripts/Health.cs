using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float Max_Health;
    public float Cur_Health;

    public bool IsInvul;
    public float PostDmgInvulDur;
    float PostDmgInvulEnd;

    public virtual void Take_Dmg(float dmg)
    {
        if (IsInvul) return;

        Cur_Health = Mathf.Clamp(Cur_Health - dmg, 0, Max_Health);
        MakeInv(PostDmgInvulDur);

        if (Cur_Health <= 0) { Death(); }
    }

    public virtual void Take_Heal(float hp)
    {
        Cur_Health = Mathf.Clamp(Cur_Health + hp, 0, Max_Health);

        if (Cur_Health <= 0) { Death(); }
    }

    public virtual void MakeInv(float t)
    {
        PostDmgInvulEnd = Time.time + t;
    }

    public virtual void Death()
    {
        Debug.Log(gameObject.name + " died");
    }

    public virtual void InfoStart()
    {
        Cur_Health = Max_Health;
    }

    public virtual void InfoUpdate()
    {
        if (Time.time >= PostDmgInvulEnd) { IsInvul = false; }
        else { IsInvul = true; }
    }
}
