using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int ItemIndex;

    bool WasPicked;
    ItemManager im;
    private void Awake()
    {
        im = FindObjectOfType<ItemManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & !WasPicked)
        {

            ItemIndex = Random.Range(1, 2);
            WasPicked = true;
            im.PickUp(ItemIndex);
            Destroy(gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {

    }
}
