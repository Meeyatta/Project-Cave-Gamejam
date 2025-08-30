using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Vector3 LookDir;
    PlayerHealth ph;
    Camera Cam;

    void Tracking()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        if (hit.point == null) return;

        Vector3 mousePos = hit.point;
        LookDir = mousePos - transform.position; LookDir.Normalize(); LookDir.y = 0;

        transform.forward = LookDir;
    }

    void Start()
    {
        Cam = Camera.main;
        ph = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ph.isDead) Tracking();
    }
}
