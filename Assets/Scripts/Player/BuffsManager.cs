using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 List of buffs:
    1 a +x health, but +y passive torch drain               V
    2 b scratch that, too hard
    3 c +x movement speed, -y max health                    V
    4 d landing attacks heals, attacks deal self damage     V
    5 e +x damage dealt, -x maximum torch power             V
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

    [Header("4 d landing attacks heals, attacks deal self damage")]
    public float d_heal; public float d_damage;

    [Header("5 e +x damage dealt, -x maximum torch power")]
    public float e_damage; public float e_power;

    [Header("6 f +x damage dealt, -x maximum health")]
    public float f_damage; public float f_health; 

    public List<Buff> CurBuffs = new List<Buff>();
    public Animator Anim;
    const string appear = "appear";
    public TextMeshProUGUI Text_;

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

        switch (ind)
        {
            case 1:
                Text_.text = "+ " + a_health + "health, but +" + a_drain + " passive torch drain";
                break;
            case 3:
                Text_.text = "+ " + c_speed + "movement speed, but -" + c_health + " maximum health";
                break;
            case 4:
                Text_.text = "Landing attacks heals " + d_heal + " health, but each attack costs " + d_damage + " health";
                break;
            case 5:
                Text_.text = "+ " + e_damage + "damage dealt, but -" + e_power + " maximum torch power";
                break;

            default:
                Text_.text = "+ " + f_damage + "damage dealt, but -" + f_health + " max health";
                break;
        }
        
        Anim.SetTrigger(appear);

        Buff bu = new Buff(ind, 1);
        CurBuffs.Add(bu);
    }

    public void ClearBuffs()
    {
        CurBuffs.Clear();
    }
}
