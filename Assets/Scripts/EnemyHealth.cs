using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    public override void Death()
    {
        Debug.Log(gameObject.name + " has died");

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
