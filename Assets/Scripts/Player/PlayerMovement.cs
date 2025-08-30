using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float Base_Speed;

    public int Dashes_Max;
    public float Base_Dash_Speed;
    public float Dash_Dur;

    [Header("-")]
    public float Dash_Gain;
    public float Dash_Cooldown;
    float Dash_Next;

    [Header("----")]
    public float Dash_Speed;
    public float Speed;

    public float Dashes_Cur;
    public bool IsDashing;
    float Dash_End;

    public Vector3 MoveDir;

    public Slider StaminaSlider;
    PlayerHealth ph;
    Camera Cam;
    CharacterController Controller;
    public Animator Anim;
    BuffsManager bm;
    const string IsMoving = "IsMoving";
    const string right = "right";
    const string left = "left";
    const string back = "back";

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        ph = GetComponent<PlayerHealth>();
        Cam = Camera.main;
        bm = GetComponent<BuffsManager>();
    }

    void Start()
    {
        Dashes_Cur = Dashes_Max;
    }
    public void BuffUpdates()
    {
        //3 c +x movement speed, -y max health      
        Speed = Base_Speed + bm.c_speed * bm.GetBuffAmount(3);
        Dash_Speed = Base_Dash_Speed + bm.c_speed * bm.GetBuffAmount(3);
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

    IEnumerator DashUpdate()
    {
        while (true)
        {
            if (Time.time > Dash_End) { IsDashing = false; }
            else { IsDashing = true; }

            Dashes_Cur = Mathf.Clamp(Dashes_Cur + Dash_Gain / 1000, 0, Dashes_Max);
            yield return new WaitForSeconds(Time.deltaTime);
        }  
    }

    bool DashCheck()
    {
        if (Dashes_Cur < 1 || Time.time < Dash_Next) return false; 

        Dashes_Cur = Mathf.Clamp(Dashes_Cur - 1, 0, Dashes_Max);
        Dash_End = Time.time + Dash_Dur;
        Dash_Next = Time.time + Dash_Cooldown;
        return true;

    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (DashCheck() && context.performed)
        {
            ph.MakeInv(Dash_Dur);
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
    private void FixedUpdate()
    {
        BuffUpdates();
    }
    void Update()
    {
        if (ph.isDead) return;

        Movement();
        StartCoroutine(DashUpdate());
        AnimationControl();

        StaminaSlider.value = Dashes_Cur / Dashes_Max;

        if (IsDashing) { Controller.Move(Dash_Speed * MoveDir * Time.deltaTime); }
        else { Controller.Move(Speed * MoveDir * Time.deltaTime); }
        
    }
}
