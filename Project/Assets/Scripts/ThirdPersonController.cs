using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    public GameObject bloodEffect;
    public GameObject Aim;
    public GameObject free_cam;
    public GameObject trail;
    public GameObject atkTrigger;
    public Cinemachine.CinemachineFreeLook cam_free_look;

    public float maxHealth = 200f;
    public float health = 200f;
    public float sp = 6f;
    public float timer = 0f;
    public float run_sp = 10f;
    public float smoothTime = 0.1f;
    public float gravity;
    public float currentGravity;
    public float maxGravity;
    public float actingTime = 0.5f;
    public bool rolling, dodging, jump,jumped, aim, prev_aim, atk, dfc, atking, turning, turn, landing, hurting;

    private int prev_state;
    private float defenceRate = 0.5f;
    private float degree = 0f;
    private float atkTime = 0f,shiftTime = 0f,hurtTime = 0f;
    private bool W,A,S,D,SHIFT,CTRL,SPACE;
    private bool dead = false;

    private Vector3 movement;
    private Vector3 gravityDic;
    private Vector3 gravityMovement;
    float horizontal;
    float vertical;
    float turnSmoothVelocity;
    float targetAngle , angle;

    public void takeDamage(float val,Vector3 pos)
    {
        if ((dodging && timer < 0.55) || (rolling && timer%1f <0.7) || hurting || hurtTime > 0 || dead) return;
        if(val > maxHealth/4) hurtTime = 0.5f;
        if (dfc) val*= defenceRate;

        GameObject blood = Instantiate(bloodEffect, pos, Quaternion.identity);
        blood.GetComponent<ParticleSystem>().Play();
        health -= val;
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void KeyInput()
    {
        W = (Input.GetKey("w") ? true : false);
        S = (Input.GetKey("s") ? true : false);
        A = (Input.GetKey("a") ? true : false);
        D = (Input.GetKey("d") ? true : false);
        SPACE = (Input.GetKey("space") ? true : false);
        CTRL = (Input.GetKey("left ctrl") ? true : false);
    }

    private void AimTheTarget()
    {
        if (aim)
        {
            degree = Mathf.Atan2(Aim.transform.position.x - transform.position.x, Aim.transform.position.z - transform.position.z) * Mathf.Rad2Deg;

            cam_free_look.m_Heading.m_Bias = degree;
            float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
            float dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
            if (dis > 25)
                aim = false;
        }

        if (aim != prev_aim)
        {
            if(aim)
            {
                angle = Mathf.SmoothDampAngle(cam.eulerAngles.y, transform.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Aim = null;

                float dist = 25f;
                foreach (var it in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    float dis_x = it.transform.position.x - transform.position.x, dis_z = it.transform.position.z - transform.position.z;
                    float dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
                    
                    if (dis < dist)
                    {
                        Aim = it;
                        dist = dis;
                    }
                }
                if (Aim == null)
                {
                    aim = false;
                    Aim = GameObject.FindGameObjectWithTag("Player");
                    cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
                }
                else
                {
                    cam_free_look.m_Lens.FieldOfView = 50;
                    cam_free_look.m_YAxis.Value = 0.2f;
                    cam_free_look.m_YAxis.m_MaxSpeed = 0;
                    cam_free_look.m_XAxis.m_MaxSpeed = 0;
                    cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.LockToTargetOnAssign;
                }
            }
            else
            {
                Aim = GameObject.FindGameObjectWithTag("Player");

                cam_free_look.m_Lens.FieldOfView = 50;
                cam_free_look.m_YAxis.m_MaxSpeed = 15;
                cam_free_look.m_XAxis.m_MaxSpeed = 800;
                cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            }
        }
        cam_free_look.LookAt = Aim.transform;
        prev_aim = aim;
    }

    private void Falling()
    {
        if (IsGrounded())
        {
            currentGravity = 0;
            animator.SetBool("IsGrounded", true);
            if (jump && timer >= 0.20 && timer <= 0.200001) currentGravity = -4f * Time.deltaTime;
        }
        else
        {
            if (currentGravity < maxGravity) { currentGravity += gravity * Time.deltaTime; }
            if (currentGravity > 0.00275) { animator.SetBool("IsGrounded", false); }
        }
        gravityMovement = gravityDic * currentGravity;
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //得到水平移動(左右)的輸入 不用判斷 a,d
        vertical = Input.GetAxisRaw("Vertical"); //得到鉛直移動(前後)的輸入 不用判斷 w,s

        Vector3 dic = new Vector3(horizontal, 0f, vertical).normalized;
        turn = false;
        if (!atking) trail.SetActive(false);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("180turn")) turning = false;
        if (dead)
        {
            movement = Vector3.zero;
            return;
        }

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
                if(IsGrounded()) movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                else movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
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
        else if (dodging && timer <= 0.6)
        {
            targetAngle = cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
        }
        else if (rolling)
        {
            if (dfc) { targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg; }
            if (aim) {
                targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
            }
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            if((animator.GetCurrentAnimatorStateInfo(0).normalizedTime%1.0) < 0.7) movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
            else movement = Vector3.zero;
        }
        else if (atking)
        {
            if (Aim.tag != "Player")
            {
                targetAngle = degree;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            }

            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("kick")) trail.SetActive(true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") && timer <= 0.56)
            {
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") && timer <= 0.6)
            {
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") && timer <= 0.4)
            {
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") && timer >= 0.4 && timer <= 0.6)
            {
                movement = Vector3.zero;
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") && timer >= 0.4 && timer <= 0.6)
            {
                movement = Vector3.zero;
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") && timer <= 0.66)
            {
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") && timer <= 0.4)
            {
                atkTrigger.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                movement = Vector3.zero;
            }
        }
        else if(IsGrounded()){ movement = Vector3.zero; }
        if(jump && timer <= 0.3) { movement = Vector3.zero;}
        if (hurting) { movement = Vector3.zero; }
        if(!atking) atkTrigger.GetComponent<BoxCollider>().enabled = false;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void AnimationInput()
    {
        timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        dead = (health <= 0 ? true : false);
        rolling = (animator.GetCurrentAnimatorStateInfo(0).IsName("roll") ? true:false);
        dodging = (animator.GetCurrentAnimatorStateInfo(0).IsName("dodge") ?true:false);
        landing = (animator.GetCurrentAnimatorStateInfo(0).IsName("landing") ? true : false);
        turning = (animator.GetCurrentAnimatorStateInfo(0).IsName("180turn")||turn ? true : false);
        hurting = (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt_lite") || animator.GetCurrentAnimatorStateInfo(0).IsName("sword_blockreact") ? true : false);
        jump = (animator.GetCurrentAnimatorStateInfo(0).IsName("jump") ? true : false);
        atking = (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") || animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("kick") ? true : false);
        aim = (Input.GetMouseButtonDown(2)?!aim:aim);
        atkTime = (Input.GetMouseButtonDown(0) ?actingTime: atkTime);
        shiftTime = (Input.GetKeyDown("left shift") ? actingTime : shiftTime);

        atk = (atkTime > 0 ? true : false);
        SHIFT = (shiftTime > 0 ? true : false);
        dfc = (Input.GetMouseButton(1) ? true : false);

        animator.SetBool("IsWalking", (W || S || A || D ? true : false));
        animator.SetBool("w", (W ? true : false));
        animator.SetBool("a", (A ? true : false));
        animator.SetBool("s", (S ? true : false));
        animator.SetBool("d", (D ? true : false));
        animator.SetBool("Dead", (dead ? true : false));
        animator.SetBool("Turning", (turning ? true : false));
        animator.SetBool("Hurt", (hurtTime > 0 ? true : false));
        animator.SetBool("Aim", (aim ? true : false));
        animator.SetBool("Atk", (atk ? true : false));
        animator.SetBool("Dfc", (dfc ? true : false));
        animator.SetBool("Space", (SPACE ? true : false));
        animator.SetBool("Shift", (SHIFT ? true : false));
        animator.SetBool("Ctrl", (CTRL ? true : false));

        if (atkTime > 0) atkTime -= 1 * Time.deltaTime;
        else atkTime = 0;
        if (shiftTime > 0) shiftTime -= 1 * Time.deltaTime;
        else shiftTime = 0;
        if (hurtTime > 0) hurtTime -= 1 * Time.deltaTime;
        else hurtTime = 0;
        if (health > maxHealth) health = maxHealth;
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
