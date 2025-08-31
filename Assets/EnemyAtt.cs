using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtt : MonoBehaviour
{
    public float Dmg;
    public float MaxRange;
    public float Cur;
    public float Addition;
    public float Max;

    PlayerHealth ph;
    // Start is called before the first frame update
    void Start()
    {
        ph = FindObjectOfType<PlayerHealth>();
    }

    private void OnEnable()
    {

        ph = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, ph.transform.position) <= MaxRange)
        {
            Cur += Addition;

            if (Cur >= Max)
            {
                ph.Take_Dmg(Dmg);
                Cur = 0;
            }
        }
        else { Cur = 0; }
    }
}
