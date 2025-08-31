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
            Debug.Log("pICKED UP A BUFF " + BuffIndex);

            BuffIndex = Random.Range(1, 6);
            while (BuffIndex == 2) { BuffIndex = Random.Range(1, 6); }

            WasPicked = true;
            bm.AddBuff(BuffIndex);
            Destroy(gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
