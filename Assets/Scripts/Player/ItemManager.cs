using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 List of active items:
    1 Oil - +x% to current torch power
    2 Mushroom - heals x health
    
 */

public class ItemManager : MonoBehaviour
{
    public int CurItemInd;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(int ind)
    {
        CurItemInd = ind;
    }

}
