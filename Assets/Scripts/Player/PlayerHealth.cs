using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("-Player specific-")]
    public float Base_MaxHealth;
    public Slider HpSlider;
    public Rigidbody rb;

    BuffsManager bm;
    public override void Death()
    {
        Debug.Log("Player has died");
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true;

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
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
        Cur_Health = Mathf.Clamp(Cur_Health, 0, Max_Health);
        HpSlider.minValue = 0;
        HpSlider.value = Cur_Health;
    }
}
