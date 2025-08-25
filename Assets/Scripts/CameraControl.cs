using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Camera Cam;
    Vector3 Offset;

    void Start()
    {
        Cam = Camera.main;
        Offset = Cam.transform.position - transform.position;
    }

    void Update()
    {
        Cam.transform.position = transform.position + Offset;
    }
}
