using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    public int Dashes_Max;
    public float Dash_Speed;
    public float Dash_Cooldown;
    public float Dash_Dur;

    [Header("-")]
    public float Dash_Between_cool;
    float Dash_Between_end;

    [Header("----")]
    public int Dashes_Cur;
    bool IsDashing;
    float Dash_CooldownEnd;
    float Dash_End;

    public Vector3 MoveDir;

    Camera Cam;
    CharacterController Controller;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Cam = Camera.main;
    }

    void Start()
    {
        Dashes_Cur = Dashes_Max;
    }

    void DashUpdate()
    {
        if (Time.time > Dash_End) { IsDashing = false; }
        else { IsDashing = true; }

        if (Dashes_Cur >= Dashes_Max) return;

        if (Time.time >= Dash_CooldownEnd)
        {
            Dashes_Cur = Mathf.Clamp(Dashes_Cur + 1, 0, Dashes_Max);
            Dash_CooldownEnd = Time.time + Dash_Cooldown;
        }
    }

    bool DashCheck()
    {
        if (Time.time <= Dash_Between_end) return false;
        Dash_Between_end = Time.time + Dash_Between_cool + Dash_Dur;

        if (Dashes_Cur <= 0) return false; 

        Dashes_Cur = Mathf.Clamp(Dashes_Cur - 1, 0, Dashes_Max);
        Dash_End = Time.time + Dash_Dur;
        return true;

    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (DashCheck() && context.performed)
        {
            IsDashing = true;
        }
    }

    void Movement()
    {
        float xi = Input.GetAxis("Horizontal");
        float yi = Input.GetAxis("Vertical");

        Vector3 camForw = Cam.transform.forward; camForw.y = 0; camForw.Normalize();
        Vector3 camRight = Cam.transform.right; camRight.y = 0; camRight.Normalize();
        MoveDir = camForw * yi + camRight * xi;

    }

    void Update()
    {
        Movement();
        DashUpdate();

        if (IsDashing) { Controller.Move(Dash_Speed * MoveDir * Time.fixedDeltaTime); }
        else { Controller.Move(Speed * MoveDir * Time.fixedDeltaTime); }
        
    }
}
