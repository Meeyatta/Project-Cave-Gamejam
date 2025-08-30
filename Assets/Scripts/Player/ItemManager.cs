using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 List of active items:
    1 Oil - +x% to current torch power
    2 Mushroom - heals x health
    
 */

public class ItemManager : MonoBehaviour
{
    public int CurItemInd;

    public float Heal;
    public float Power;
    public float Time;

    public GameObject OilPng;
    public GameObject MushroomPng;

    PlayerHealth hp;
    TorchManager tm;
    private void Awake()
    {
        hp = GetComponent<PlayerHealth>();
        tm = GetComponent<TorchManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurItemInd == 1) { OilPng.SetActive(true); MushroomPng.SetActive(false); }
        else if (CurItemInd == 2) { OilPng.SetActive(false); MushroomPng.SetActive(true); }
        else { OilPng.SetActive(false); MushroomPng.SetActive(false); }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CurItemInd == 0) return;

            if (CurItemInd == 2) 
            {
                hp.Take_Heal(Heal);
            }
            else
            {
                StartCoroutine(tm.Power_Add(Power, Time));
            }

            CurItemInd = 0;
            MushroomPng.SetActive(false);
            OilPng.SetActive(false);
        }
    }

    public void PickUp(int ind)
    {
        CurItemInd = ind;

    }

}
