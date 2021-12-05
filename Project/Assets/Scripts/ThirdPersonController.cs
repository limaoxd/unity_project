using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    public GameObject trail;
    public GameObject Aim;
    public GameObject free_cam;
    public Cinemachine.CinemachineFreeLook cam_free_look;

    public float sp = 6f;
    public float run_sp = 10f;
    public float smoothTime = 0.1f;
    public float gravity;
    public float currentGravity;
    public float maxGravity;
    public float actingTime = 0.5f;
    public bool rolling, dodging, jump, aim, prev_aim, atk, dfc, atking, hurt, turning, turn, landing;
    private float degree = 0f;
    private float atkTime = 0f;
    private bool W,A,S,D,SHIFT,CTRL,SPACE;

    private Vector3 movement;
    private Vector3 gravityDic;
    private Vector3 gravityMovement;
    float horizontal;
    float vertical;
    float turnSmoothVelocity;
    float targetAngle  , angle;

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

    private void AimTheTarget()
    {
        if(aim != prev_aim)
        {
            if(aim)
            {
                angle = Mathf.SmoothDampAngle(cam.eulerAngles.y, transform.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Aim = GameObject.FindGameObjectWithTag("Enemy");
                if (Aim == null)
                    aim = false;
          
                cam_free_look.m_XAxis.m_MaxSpeed = 0;
                cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.LockToTargetOnAssign;
            }
            else
            {
                Aim = GameObject.FindGameObjectWithTag("Player");
                cam_free_look.m_XAxis.m_MaxSpeed = 800;
                cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            }
        }
        if (aim)
        {
            degree = Mathf.Atan2(Aim.transform.position.x - transform.position.x, Aim.transform.position.z - transform.position.z) * Mathf.Rad2Deg;
            if(degree + 225 <= 180) cam_free_look.m_Heading.m_Bias = degree+225;
            else cam_free_look.m_Heading.m_Bias = -135 + degree;
        }
        cam_free_look.LookAt = Aim.transform;
        prev_aim = aim;
    }

    private void Falling()
    {
        if (IsGrounded()) {
            currentGravity = 0;
            animator.SetBool("IsGrounded",true);
            if (jump && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.20 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.21) currentGravity = -0.03f;
        }
        else {
            if (currentGravity < maxGravity) { currentGravity += gravity * Time.deltaTime;}
            if (currentGravity > 0.01) { animator.SetBool("IsGrounded", false); }
        }
        gravityMovement = gravityDic * currentGravity;
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //得到水平移動(左右)的輸入 不用判斷 a,d
        vertical = Input.GetAxisRaw("Vertical"); //得到鉛直移動(前後)的輸入 不用判斷 w,s
        Vector3 dic = new Vector3(horizontal, 0f, vertical).normalized;
        turn = false;
        if(!atking) trail.SetActive(false);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("180turn")) turning = false;

        if (landing)
        {
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            movement = Vector3.zero;
        }
        else if (turning)
        {
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized *sp * Time.deltaTime;
        }
        else if (dic.magnitude >= 0.1f && !dodging && !rolling && !atking && !dfc)
        {
            targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg; //actan 函數 , 將 x,y 輸入後能夠得到對應角 (徑度) 在通過轉換變成角度
            if (CTRL)
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
                if (Mathf.Abs((targetAngle + 360) % 360 - (transform.eulerAngles.y + 360) % 360) >= 170 && Mathf.Abs((targetAngle + 360) % 360 - (transform.eulerAngles.y + 360) % 360) <= 190) { turn = true; }
                movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
            }
            else if (!aim)
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
                movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else
            {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
                float walkangle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
                movement = Quaternion.Euler(0f,walkangle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
        }
        else if (turning) { movement = Vector3.zero; }
        else if (dodging && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6)
        {
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
        }
        else if (rolling && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7)
        {
            if (dfc) { targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg; }
            if (aim) {
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3) targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
        }
        else if (atking)
        {
            if(Aim.tag != "Player")
            {
                targetAngle = degree;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            }

            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("kick")) trail.SetActive(true);
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
        if(jump && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3) { movement = Vector3.zero;}

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void AnimationInput()
    {
        rolling = (animator.GetCurrentAnimatorStateInfo(0).IsName("roll") ? true:false);
        dodging = (animator.GetCurrentAnimatorStateInfo(0).IsName("dodge") ?true:false);
        landing = (animator.GetCurrentAnimatorStateInfo(0).IsName("landing") ? true : false);
        turning = (animator.GetCurrentAnimatorStateInfo(0).IsName("180turn")||turn ? true : false);
        jump = (animator.GetCurrentAnimatorStateInfo(0).IsName("jump") ? true : false);
        atking = (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") || animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("kick") ? true : false);
        aim = (Input.GetMouseButtonDown(2)?!aim:aim);
        atkTime = (Input.GetMouseButtonDown(0) ?actingTime: atkTime);
        atk = (atkTime>0 ? true : false);
        dfc = (Input.GetMouseButton(1) ? true : false);
        animator.SetBool("IsWalking",(W || S || A || D ? true : false));
        animator.SetBool("w", (W ? true : false));
        animator.SetBool("a", (A ? true : false));
        animator.SetBool("s", (S ? true : false));
        animator.SetBool("d", (D ? true : false));
        animator.SetBool("Turning", (turning ? true : false));
        animator.SetBool("Aim", (aim ? true : false));
        animator.SetBool("Atk", (atk ? true : false));
        animator.SetBool("Dfc", (dfc ? true : false));
        animator.SetBool("Space", (SPACE ? true : false));
        animator.SetBool("Shift", (SHIFT ? true : false));
        animator.SetBool("Ctrl", (CTRL ? true : false));

        if (atkTime > 0) atkTime -= 1 * Time.deltaTime;
        else atkTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        gravityDic = Vector3.down;
        Aim = GameObject.FindGameObjectWithTag("Player");
  
        cam_free_look = free_cam.GetComponent<Cinemachine.CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        AnimationInput();
        AimTheTarget();
        Falling();
        Movement();

        controller.Move(gravityMovement+movement);
    }
}
