using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 List of buffs:
    1 a +x health, but +y passive torch drain               V
    2 b enemies explode on death, deal x damage
    3 c +x movement speed, -y max health                    X
    4 d enemies drop healing, attacks deal self damage
    5 e +x damage dealt, -x maximum torch power
    6 f +x damage dealt, -x maximum health                  V
 */
[System.Serializable]
public class Buff
{
    public int Index;
    public int Amount;

    public Buff(int i, int a)
    {
        this.Index = i; this.Amount = a;
    }
}

public class BuffsManager : MonoBehaviour
{
    [Header("1 a +x health, but +y passive torch drain")]
    public float a_health; public float a_drain;

    [Header("3 c +x movement speed, -y max health")]
    public float c_speed; public float c_health;

    [Header("6 f +x damage dealt, -x maximum health")]
    public float f_damage; public float f_health; 

    public List<Buff> CurBuffs = new List<Buff>();

    public int GetBuffAmount(int ind)
    {
        foreach (var b in CurBuffs) { if (b.Index == ind) { return b.Amount; } }
        
        return 0;
    }

    public void AddBuff(int ind) 
    { 
        foreach (var b in CurBuffs)
        {
            if (b.Index == ind) { b.Amount++; return; }
        }

        Buff bu = new Buff(ind, 1);
        CurBuffs.Add(bu);
    }

    public void ClearBuffs()
    {
        CurBuffs.Clear();
    }
}
