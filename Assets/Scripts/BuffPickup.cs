using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickup : MonoBehaviour
{
    public int BuffIndex;

    bool WasPicked;
    BuffsManager bm;
    private void Awake()
    {
        bm = FindObjectOfType<BuffsManager>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & !WasPicked)
        {
            WasPicked = true;
            bm.AddBuff(BuffIndex);
            Destroy(gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
