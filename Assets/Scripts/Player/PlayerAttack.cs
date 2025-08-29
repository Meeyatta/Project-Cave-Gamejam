using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float Base_Damage;
    public float Attack_Cooldown;
    public float Attack_Next;
    [Header("---")]
    public float Damage;
    bool Attacked1 = false;
    public DamageTrigger Trigger;
    public Animator Anim;
    PlayerMovement pm;
    PlayerHealth hp;
    BuffsManager bm;

    const string attack1 = "attack1";
    const string attack2 = "attack2";
    const string AttackLayer = "AttackLayer";
    int ind;

    public bool CanAttack() 
    {
        return !pm.IsDashing;
    }

    public void iAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CanAttack()) Attack();
        }
    }

    void Attack()
    {
        //if (Trigger.CurTargets == null || Trigger.CurTargets.Count <= 0) return;
        if (Time.time < Attack_Next) return;

        //4 d landing attacks heals, attacks deal self damage
        if (bm.GetBuffAmount(4) > 0) { hp.Take_Dmg(bm.d_damage); }

        Anim.SetLayerWeight(ind, 1);
        if (Attacked1) { Anim.SetTrigger(attack1); } else { Anim.SetTrigger(attack2); }
        Attacked1 = !Attacked1;
        Attack_Next = Time.time + Attack_Cooldown;
    }
    public void BuffUpdates()
    {
                            //6 f +x damage dealt, -x maximum health //5 e +x damage dealt, -x maximum torch power
        Damage = Base_Damage + bm.f_damage * bm.GetBuffAmount(6)     + bm.e_damage * bm.GetBuffAmount(5);

        //5 e +x damage dealt, -x maximum torch power
    }

    void Start()
    {
        ind = Anim.GetLayerIndex(AttackLayer);
        pm = GetComponent<PlayerMovement>();
        bm = GetComponent<BuffsManager>();
        hp = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        BuffUpdates();
    }

    void Update()
    {
        
    }
}
