using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public GameObject Aim;
    public GameObject trail;
    public GameObject bloodEffect;
    public GameObject atkTrigger;
    public NavMeshAgent agent;
    public Image Health_bar;
    public Image Health_load;

    public float           maxHealth = 100;
    public float           Health = 100;
    public float           sp = 2.5f;
    public float           run_sp = 6f;
    public float           smoothTime = 0.1f;
    public float           gravity = 0.1f;
    public float           currentGravity;
    public float           maxGravity = 5.0f;
    public float           Damage = 0f;
    public float           poise = 100.0f;
    public float           stamina = 100.0f;

    public int             atk_n = 0;

    protected float        player_dis = 0f , hurtTime = 0f;
    protected bool[]       atk_state;
    protected bool         dead , atking , dodge ,atked;
    protected bool         p_atking;
    protected string[]     atk;

    protected Vector3      movement;
    protected Vector3      gravityDic;
    protected Vector3      gravityMovement;
    protected float        turnSmoothVelocity;
    protected float        healthTurnSmoothVelocity;
    protected float        targetAngle, angle;

    public void takeDamage(float val,Vector3 pos)
    {
        if (dead || hurtTime > 0) return;

        hurtTime = 0.5f;
        GameObject blood = Instantiate(bloodEffect, pos, Quaternion.identity);
        blood.GetComponent<ParticleSystem>().Play();
        poise -= 200*val/maxHealth;
        Health -= val;
    }

    protected bool IsGrounded()
    {
        return controller.isGrounded;
    }

    protected void Set_state()
    {
        float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
        targetAngle = Mathf.Atan2(dis_x, dis_z) * Mathf.Rad2Deg;
        player_dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
        p_atking = Aim.GetComponent<ThirdPersonController>().atking;

        if (hurtTime > 0) hurtTime -= 1 * Time.deltaTime;
        else hurtTime = 0;
        if (stamina >= 100f) { stamina = 100f; }
        else { stamina += 2f * Time.deltaTime; }
        if (poise >= 100f) { poise = 100f; }
        else { poise += 0.2f * Time.deltaTime; }
        if (Health <= 0) dead = true;
    }

    protected void Falling()
    {
        if (IsGrounded()) {currentGravity = 0;}
        else if (currentGravity < maxGravity) { currentGravity += gravity * Time.deltaTime; }
        gravityMovement = gravityDic * currentGravity;
    }

    // Start is called before the first frame update
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        gravityDic = Vector3.down;
        atk_state = new bool[atk_n];
        atk = new string[atk_n];
    }

    public void Healthbar()
    {
        if (Health > maxHealth) Health = maxHealth;
        Health_bar.fillAmount = Health / maxHealth;
        Health_load.fillAmount = Mathf.SmoothDampAngle(Health_load.fillAmount, Health_bar.fillAmount, ref healthTurnSmoothVelocity, 3 * smoothTime);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Falling();
        controller.Move(gravityMovement + movement);
    }
}
