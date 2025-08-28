using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [Header("-Player specific-")]
    public Slider HpSlider;
    public override void Death()
    {
        Debug.Log("Player has died");
    }
    private void Start()
    {
        InfoStart();
    }

    private void Update()
    {
        InfoUpdate();

        HpSlider.maxValue = Max_Health;
        HpSlider.minValue = 0;
        HpSlider.value = Cur_Health;
    }
}
