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
    public Animator Anim;
    const string IsMoving = "IsMoving";
    const string right = "right";
    const string left = "left";
    const string back = "back";

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Cam = Camera.main;
    }

    void Start()
    {
        Dashes_Cur = Dashes_Max;
    }

    void AnimationControl()
    {
        Vector3 localMove = transform.InverseTransformDirection(MoveDir);

        if (localMove != Vector3.zero)
        {
            Anim.SetBool(IsMoving, true);

            if (localMove.z > 0) { Anim.SetBool(back, false); }
            else if (localMove.z < 0) { Anim.SetBool(back, true); }

            if (localMove.x > 0 && Mathf.Abs(localMove.x) > Mathf.Abs(localMove.z))
            {
                Anim.SetBool(right, true);
                Anim.SetBool(left, false);
            }
            else if (localMove.x < 0 && Mathf.Abs(localMove.x) > Mathf.Abs(localMove.z))
            {
                Anim.SetBool(left, true);
                Anim.SetBool(right, false);
            }
            else
            {
                Anim.SetBool(left, false);
                Anim.SetBool(right, false);
            }
        }
        else
        {
            Anim.SetBool(IsMoving, false);
            Anim.SetBool(left, false);
            Anim.SetBool(right, false);
            Anim.SetBool(back, false);
        }
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
        AnimationControl();

        if (IsDashing) { Controller.Move(Dash_Speed * MoveDir * Time.deltaTime); }
        else { Controller.Move(Speed * MoveDir * Time.deltaTime); }
        
    }
}
