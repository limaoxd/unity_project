using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    public float sp = 6f;
    public float run_sp = 10f;
    public float gravity;
    public float currentGravity;
    public float maxGravity;
    private bool W,A,S,D,SHIFT,CTRL,SPACE;
    private bool rolling , dodging , jump ,aim ,atk , dfc , atking , hurt;

    private Vector3 movement;
    private Vector3 gravityDic;
    private Vector3 gravityMovement;
    float horizontal;
    float vertical;
    float targetAngle;

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void KeyInput()
    {
        W = (Input.GetKey("w") ?true:false);
        S = (Input.GetKey("s") ? true : false);
        A = (Input.GetKey("a") ? true : false);
        D = (Input.GetKey("d") ? true : false);
        SHIFT = (Input.GetKey("left shift") ? true : false);
        SPACE = (Input.GetKey("space") ? true : false);
        CTRL = (Input.GetKey("left ctrl") ? true : false);
    }

    private void Falling()
    {
        if (IsGrounded()) {
            currentGravity = 0;
            animator.SetBool("IsGrounded",true);
            if (jump && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.20 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.21) currentGravity = -0.045f;
        }
        else {
            if (currentGravity < maxGravity) { currentGravity += gravity * Time.deltaTime;}
            if (currentGravity > 0.1) { animator.SetBool("IsGrounded", false); }
        }
        gravityMovement = gravityDic * currentGravity;
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //得到水平移動(左右)的輸入 不用判斷 a,d
        vertical = Input.GetAxisRaw("Vertical"); //得到鉛直移動(前後)的輸入 不用判斷 w,s
        Vector3 dic = new Vector3(horizontal, 0f, vertical).normalized;
        if (dic.magnitude >= 0.1f && !dodging && !rolling && !atking && !dfc)
        {
            if (!aim)
            {
                targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg; //actan 函數 , 將 x,y 輸入後能夠得到對應角 (徑度) 在通過轉換變成角度
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                if (!CTRL) { movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
            }
            else
            {
                targetAngle = cam.eulerAngles.y;
                if (!CTRL)
                {
                    if (W){movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;}
                    if (S){movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * Time.deltaTime;}
                    if (A){movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.left.normalized * sp * Time.deltaTime;}
                    if (D){ movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.right.normalized * sp * Time.deltaTime;}
                }
                else
                {
                    targetAngle += Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
                    movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                }
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
        }
        else if (dodging && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6)
        {
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
        }
        else if (rolling && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7)
        {
            if (dfc) { targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg; }
            if (aim) {
                if(dic.magnitude >= 0.1f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3) targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
        }
        else if (atking)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.56)
            {
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6)
            {
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4)
            {
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.66)
            {
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else
            {
                movement = Vector3.zero;
            }
        }
        else if(IsGrounded()){ movement = Vector3.zero; }
    }

    private void AnimationInput()
    {
        rolling = (animator.GetCurrentAnimatorStateInfo(0).IsName("roll") ? true:false);
        dodging = (animator.GetCurrentAnimatorStateInfo(0).IsName("dodge") ?true:false);
        jump = (animator.GetCurrentAnimatorStateInfo(0).IsName("jump") ? true : false);
        atking = (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") || animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("kick") ? true : false);
        aim = (Input.GetMouseButtonDown(2)?!aim:aim);
        atk = (Input.GetMouseButton(0) ? true : false);
        dfc = (Input.GetMouseButton(1) ? true : false);
        animator.SetBool("IsWalking",(W || S || A || D ? true : false));
        animator.SetBool("w", (W ? true : false));
        animator.SetBool("a", (A ? true : false));
        animator.SetBool("s", (S ? true : false));
        animator.SetBool("d", (D ? true : false));
        animator.SetBool("Aim", (aim ? true : false));
        animator.SetBool("Atk", (atk ? true : false));
        animator.SetBool("Dfc", (dfc ? true : false));
        animator.SetBool("Space", (SPACE ? true : false));
        animator.SetBool("Shift", (SHIFT ? true : false));
        animator.SetBool("Ctrl", (CTRL ? true : false));
    }

    // Start is called before the first frame update
    void Start()
    {
        gravityDic = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        AnimationInput();
        Falling();
        Movement();

        controller.Move(gravityMovement+movement);
    }
}
