using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    public GameObject bloodEffect;
    public GameObject Aim;
    public GameObject free_cam;
    public GameObject aim_point;
    public GameObject trail;
    public atk_trigger atkTrigger;
    public atk_trigger lastAtkTrigger;
    public Image Hp_bar;
    public Image Hp_load;
    public Image Sp_bar;
    public Image Sp_load;
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip[] audios;

    public int drinkMax = 5;
    public int drinkLeft;
    public float maxHealth = 400f,maxStamina = 150f;
    public float health = 400f,stamina = 150f;
    public float heal;
    public float sp = 6f;
    public float timer = 0f;
    public float run_sp = 10f;
    public float smoothTime = 0.1f;
    public float gravity;
    public float currentGravity;
    public float maxGravity;
    public float actingTime = 0.5f;
    public float hurtTime = 0f;
    public bool rolling, dodging, jump,jumped, aim, prev_aim, atk, dfc, atking, turning, drinking, turn, landing, hurting,waking,menuing = false, dead = false;
    private Cinemachine.CinemachineFreeLook cam_free_look;
    private Transform point_to_aim;
    private int prev_state;
    private float defenceRate = 0.5f;
    private float degree = 0f , ini_degree = 0f;
    private float atkTime = 0f,shiftTime = 0f;
    private bool W,A,S,D,R,SHIFT,CTRL,SPACE;
    private bool acted = false , drinked =  false , hausted = false;
    private Vector3 movement;
    private Vector3 gravityDic;
    private Vector3 gravityMovement;
    float horizontal;
    float vertical;
    float turnSmoothVelocity;
    float hpTurnSmoothVelocity,hp1TurnSmoothVelocity,spTurnSmoothVelocity;
    float targetAngle , angle;

    public void takeDamage(float val,Vector3 pos)
    {
        if ((dodging && timer < 0.55) || (rolling && timer%1f <0.7) || hurting || hurtTime > 0 || dead) return;
        hurtTime = 0.5f;
        if (dfc) val*= defenceRate;

        audioSource.PlayOneShot(audios[Random.Range(0,audios.Length)]);
        GameObject blood = Instantiate(bloodEffect, pos, Quaternion.identity);
        blood.GetComponent<ParticleSystem>().Play();
        health -= val;
    }

    public void Reset_state(){
        drinkLeft = drinkMax;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        dead = false;
        heal = maxHealth / 3;
        health = maxHealth;
        stamina = maxStamina;
    }

    private bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down.normalized, 0.1f)) return true;
        return false;
    }

    private void KeyInput()
    {
        atkTrigger = this.GetComponentInChildren<atk_trigger>();
        if(lastAtkTrigger != atkTrigger) trail = GameObject.FindGameObjectWithTag("Weapon");

        lastAtkTrigger = atkTrigger;

        W = (Input.GetKey("w") ? true : false);
        S = (Input.GetKey("s") ? true : false);
        A = (Input.GetKey("a") ? true : false);
        D = (Input.GetKey("d") ? true : false);
        R = (Input.GetKey("r") ? true : false);
        SPACE = (Input.GetKey("space") && stamina >= 10? true : false);
        if(stamina <= 0) hausted = true;
        else if(stamina >= 50f) hausted = false;
        CTRL = (Input.GetKey("left ctrl") && !hausted ? true : false);
    }

    public bool Aimset(){
        aim = true;
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
            return false;
        }
        else
        {
            cam_free_look.m_Lens.FieldOfView = 50 * Aim.GetComponent<AI>().c_dis;
            cam_free_look.m_YAxis.Value = 0.2f;
            cam_free_look.m_YAxis.m_MaxSpeed = 0;
            cam_free_look.m_XAxis.m_MaxSpeed = 0;
            cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.LockToTargetOnAssign;
        }
        if(Aim.GetComponentInChildren<bar_on_head>())
            Aim.GetComponentInChildren<bar_on_head>().gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 15);

        foreach(var it in GameObject.FindGameObjectsWithTag("Aim_point"))
            if(it.GetComponentInParent<AI>().gameObject == Aim)
                point_to_aim = it.transform;
        return true;
    }

    private void AimTheTarget()
    {
        if (aim != prev_aim)
        {
            if(aim) Aimset();
            else
                foreach(var it in GameObject.FindGameObjectsWithTag("Bar"))
                    it.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        if (aim && Aim.tag!="Player")
        {
            aim_point.SetActive(true);
            aim_point.transform.position = Camera.main.WorldToScreenPoint(point_to_aim.position);
            degree = Mathf.Atan2(Aim.transform.position.x - transform.position.x, Aim.transform.position.z - transform.position.z) * Mathf.Rad2Deg;

            if(degree - ini_degree < -180) cam_free_look.m_Heading.m_Bias = degree - ini_degree + 360;
            cam_free_look.m_Heading.m_Bias = degree - ini_degree;


            float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
            float dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
            if ((dis > 25 || Aim.tag!="Enemy")&&!Aimset()) aim = false;
                
        }
        else
        {
            aim_point.SetActive(false);
            point_to_aim = null;
            Aim = GameObject.FindGameObjectWithTag("Player");

            cam_free_look.m_Lens.FieldOfView = 50;
            cam_free_look.m_YAxis.m_MaxSpeed = 7;
            cam_free_look.m_XAxis.m_MaxSpeed = 350;
            cam_free_look.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
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
            if (jump && !jumped && timer < 0.5d) {
                stamina -= 15;
                currentGravity = -0.08f;
                jumped = true;
            }else jumped = false;
        }
        else
        {
            if (currentGravity < maxGravity) currentGravity += gravity * Time.deltaTime;
            animator.SetBool("IsGrounded", false);
        }
        gravityMovement = gravityDic * currentGravity;
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 dic = new Vector3(horizontal, 0f, vertical).normalized;
        turn = false;
        if (!atking && trail) trail.SetActive(false);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("180turn")) turning = false;

        if (dead || waking)
        {
            atkTrigger.atk = false;
            movement = Vector3.zero;
            controller.enabled = false;
            aim = false;
            return;
        }
        else controller.enabled = true;

        if (landing)
        {
            atkTrigger.atk = false;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            movement = Vector3.zero;
        }
        else if (turning)
        {
            atkTrigger.atk = false;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
        }
        else if (drinking)
        {

            atkTrigger.atk = false;
            movement = Vector3.zero;
            if(!drinked && timer <=0.3f) {
                health += heal;
                drinked = true;
                drinkLeft--;
            }
            else if(timer > 0.3f)
                drinked = false;
        }
        else if (dic.magnitude >= 0.1f && !dodging && !rolling && !atking && !dfc)
        {
            atkTrigger.atk = false;
            targetAngle = cam.eulerAngles.y + Mathf.Atan2(dic.x, dic.z) * Mathf.Rad2Deg;
            if (CTRL)
            {
                stamina -= 35f*Time.deltaTime;
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
            atkTrigger.atk = false;
            if(timer <=0.4f&&!acted) {
                stamina -= 30f;
                acted = true;
            }else if(timer >0.4f) acted = false;

            targetAngle = cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, smoothTime);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
        }
        else if (rolling)
        {
            atkTrigger.atk = false;
            if(timer <=0.4f&&!acted) {
                stamina -= 35f;
                acted = true;
            }else if(timer >0.4f)acted = false;

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
            if(timer <=0.5f&&!acted) {
                stamina -= 25f;
                acted = true;
            }else if(timer >0.5f)acted = false;

            if (Aim.tag != "Player")
            {
                targetAngle = degree;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            }
            
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("kick")) trail.SetActive(true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") && timer <= 0.56)
            {
                atkTrigger.atk = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") && timer <= 0.6)
            {
                atkTrigger.atk = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") && timer <= 0.4)
            {
                atkTrigger.atk = true;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if((animator.GetCurrentAnimatorStateInfo(0).IsName("atk1")&& timer <= 0.25 || animator.GetCurrentAnimatorStateInfo(0).IsName("atk2")) && timer <= 0.25){
                atkTrigger.atk = false;
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") && timer > 0.25 && timer <= 0.6)
            {
                atkTrigger.atk = true;
                movement =  Vector3.zero;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") && timer >= 0.4 && timer <= 0.6)
            {
                atkTrigger.atk = true;
                movement =  Vector3.zero;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") && (timer <= 0.2 || timer >0.6))
            {
                atkTrigger.atk = false;
                movement =  Vector3.zero;
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("atk4")){
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
                atkTrigger.atk = true;
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") && timer <= 0.4)
            {
                atkTrigger.atk = true;
            }
            else
            {
                atkTrigger.atk = false;
                movement = Vector3.zero;
            }
        }
        else if(IsGrounded()){ movement = Vector3.zero;acted = false; }
        if(jump && timer <= 0.3) movement = Vector3.zero;
        if (hurting) { atkTrigger.atk = false ; movement = Vector3.zero; }

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
        waking = (animator.GetCurrentAnimatorStateInfo(0).IsName("stand_up") ? true:false);
        jump = (animator.GetCurrentAnimatorStateInfo(0).IsName("jump") ? true : false);
        drinking = (animator.GetCurrentAnimatorStateInfo(0).IsName("drink") ? true : false);
        atking = (animator.GetCurrentAnimatorStateInfo(0).IsName("atk1") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk2") || animator.GetCurrentAnimatorStateInfo(0).IsName("atk4") || animator.GetCurrentAnimatorStateInfo(0).IsName("rolling_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("run_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("spin_atk") || animator.GetCurrentAnimatorStateInfo(0).IsName("kick") ? true : false);
        aim = (Input.GetMouseButtonDown(2)?!aim:aim);
        atkTime = (Input.GetMouseButtonDown(0) && stamina >= 25?actingTime: atkTime);
        shiftTime = (Input.GetKeyDown("left shift") && stamina >= 27 ? actingTime : shiftTime);

        atk = (atkTime > 0 ? true : false);
        SHIFT = (shiftTime > 0 ? true : false);
        dfc = (Input.GetMouseButton(1) ? true : false);

        animator.SetBool("IsWalking", (W || S || A || D ? true : false));
        animator.SetBool("w", (W ? true : false));
        animator.SetBool("a", (A ? true : false));
        animator.SetBool("s", (S ? true : false));
        animator.SetBool("d", (D ? true : false));
        animator.SetBool("r", (R && drinkLeft > 0 ? true : false));
        animator.SetBool("Dead", (dead ? true : false));
        animator.SetBool("Turning", (turning ? true : false));
        animator.SetBool("Hurt", (hurtTime > 0 ? true : false));
        animator.SetBool("Aim", (aim ? true : false));
        animator.SetBool("Atk", (atk && !menuing ? true : false));
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
    }

    void Bar()
    {
        if (health > maxHealth) health = maxHealth;
        if (Hp_bar.fillAmount < health / maxHealth) Hp_bar.fillAmount = Mathf.SmoothDampAngle(Hp_bar.fillAmount, health/maxHealth , ref hpTurnSmoothVelocity, 0.15f);
        else Hp_bar.fillAmount = health / maxHealth;
        Hp_load.fillAmount = Mathf.SmoothDampAngle(Hp_load.fillAmount, Hp_bar.fillAmount, ref hp1TurnSmoothVelocity, 0.3f);
        
        if (stamina > maxStamina) stamina = maxStamina;
        if (stamina < maxStamina && Sp_load.fillAmount - Sp_bar.fillAmount < 0.01f) stamina += maxStamina/5f*Time.deltaTime;
        if(stamina <= 0) stamina = 0;
        Sp_bar.fillAmount = stamina/maxStamina;
        Sp_load.fillAmount = Mathf.SmoothDampAngle(Sp_load.fillAmount, Sp_bar.fillAmount, ref spTurnSmoothVelocity, 0.3f);
    }

    // Start is called before the first frame update
    void Start()
    {
        gravityDic = Vector3.down;
        ini_degree = transform.eulerAngles.y;
        targetAngle = ini_degree;
        angle = targetAngle;
        Aim = GameObject.FindGameObjectWithTag("Player");
        heal = maxHealth / 3;
        drinkLeft = drinkMax;
  
        cam_free_look = free_cam.GetComponent<Cinemachine.CinemachineFreeLook>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        AnimationInput();
        Bar();
        AimTheTarget();
        Falling();
        Movement();

        controller.Move(gravityMovement+movement);
    }
}
