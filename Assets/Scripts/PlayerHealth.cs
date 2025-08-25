using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
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
    }
}
