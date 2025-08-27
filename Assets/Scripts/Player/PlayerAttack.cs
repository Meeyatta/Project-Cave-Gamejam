using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float Damage;
    public float Attack_Cooldown;
    public float Attack_Next;
    [Header("---")]
    bool Attacked1 = false;
    public DamageTrigger Trigger;
    public Animator Anim;

    const string AttackLayer = "Attack Layer";
    const string attack1 = "attack1";
    const string attack2 = "attack2";

    public void iAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Trigger.CurTargets == null || Trigger.CurTargets.Count <= 0) return;
        if (Time.time < Attack_Next) return;

        int ind = Anim.GetLayerIndex(AttackLayer);
        Anim.SetLayerWeight(ind, 1);

        if (Attacked1) { Anim.SetTrigger(attack1); } else { Anim.SetTrigger(attack2); }
        Attacked1 = !Attacked1;
        

        foreach (var v in Trigger.CurTargets)
        {
            v.Hp.Take_Dmg(Damage);
        }
        Attack_Next = Time.time + Attack_Cooldown;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
