using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public GameObject Aim;
    public GameObject trail;

    public float           Health = 100;
    public float           sp = 2.5f;
    public float           run_sp = 6f;
    public float           smoothTime = 0.1f;
    public float           gravity = 0.1f;
    public float           currentGravity;
    public float           maxGravity = 5.0f;
    public float           poise = 100.0f;
    public float           stamina = 100.0f;

    public int             atk_n = 0;

    protected float        player_dis = 0;
    protected bool[]       atk_state;
    protected bool         dead , atking , dodge ,atked;
    protected bool         p_atking;
    protected string[]     atk;

    protected Vector3      movement;
    protected Vector3      gravityDic;
    protected Vector3      gravityMovement;
    protected float        turnSmoothVelocity;
    protected float        targetAngle, angle;

    protected bool IsGrounded()
    {
        return controller.isGrounded;
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
        gravityDic = Vector3.down;
        atk_state = new bool[atk_n];
        atk = new string[atk_n];
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Falling();
        controller.Move(gravityMovement + movement);
    }
}
