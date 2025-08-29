using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [Header("-Player specific-")]
    public float Base_MaxHealth;
    public Slider HpSlider;

    BuffsManager bm;
    public override void Death()
    {
        Debug.Log("Player has died");
    }
    private void Start()
    {
        InfoStart();
        bm = GetComponent<BuffsManager>();
    }

    //I know this is the worst way to check for buffs, but it's the most straight forward one
    public void BuffUpdates()
    {
                             //1 a +x health, but +y passive torch drain //6 f +x damage dealt, -x maximum health  //3 c +x movement speed, -y max health
        Max_Health = Base_MaxHealth + bm.a_health * bm.GetBuffAmount(1)  + bm.f_health * bm.GetBuffAmount(6)       - bm.c_health * bm.GetBuffAmount(3);
    }

    private void FixedUpdate()
    {
        BuffUpdates();
    }

    private void Update()
    {
        InfoUpdate();

        HpSlider.maxValue = Max_Health;
        HpSlider.minValue = 0;
        HpSlider.value = Cur_Health;
    }
}
