using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float Damage;
    public float Attack_Cooldown;
    float Attack_Next;
    [Header("---")]
    public DamageTrigger Trigger;

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
