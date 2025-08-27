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

    const string attack1 = "attack1";
    const string attack2 = "attack2";
    const string AttackLayer = "AttackLayer";
    int ind;

    public void iAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    void Attack()
    {
        //if (Trigger.CurTargets == null || Trigger.CurTargets.Count <= 0) return;
        if (Time.time < Attack_Next) return;

        Anim.SetLayerWeight(ind, 1);
        if (Attacked1) { Anim.SetTrigger(attack1); } else { Anim.SetTrigger(attack2); }
        Attacked1 = !Attacked1;
        Attack_Next = Time.time + Attack_Cooldown;
    }

    void Start()
    {
        ind = Anim.GetLayerIndex(AttackLayer);
    }

    void Update()
    {
        
    }
}
