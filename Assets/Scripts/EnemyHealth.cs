using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    public override void Death()
    {
        if (isDead) return;
        base.Death();

        Debug.Log(gameObject.name + " has died");

        EnemyDrop dropSystem = GetComponent<EnemyDrop>();
        if (dropSystem != null)
        {
            dropSystem.TryDropItem();
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        InfoStart();
    }

    private void Update()
    {
        InfoUpdate();
    }
}
