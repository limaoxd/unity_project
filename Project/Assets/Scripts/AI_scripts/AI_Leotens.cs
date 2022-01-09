using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Leotens : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    public GameObject trail1;
    public GameObject atkTrigger1;

    void build()
    {
        map.Add(Animator.StringToHash("Idle"), 0);
        map.Add(Animator.StringToHash("Walk"), 1);
        map.Add(Animator.StringToHash("Cast"), 2);
        map.Add(Animator.StringToHash("Atk0"), 3);
        map.Add(Animator.StringToHash("Atk1"), 4);
        map.Add(Animator.StringToHash("Atk2"), 5);
        map.Add(Animator.StringToHash("Atk3"), 6);
        map.Add(Animator.StringToHash("Death"), 7);
    }

    void choose(float timer , int act_num){
        if (timer < 0.5) {atking = true; atked = false;}
        if (!atked && timer >= 0.5 && timer < 0.55) choose_atk(act_num);
        else if (timer >= 0.9) atked = false;
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < atk_n; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 5) return; //return to idle
        stamina -= 20+Random.Range(-1,5);
        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if(act_num >= 3 && act_num <= 6) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count)];
        else choosen = Random.Range(0,4);

        switch (choosen)
        {
            case 0:
                Damage = 130;
                break;
            case 1:
                Damage = 150;
                break;
            case 2:
                Damage = 150;
                break;
            case 3:
                Damage = 150;
                break;
        }
        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        atkTrigger1.GetComponent<atk_trigger>().Damage = this.Damage;
        atked = true;
        atk_state[choosen] = true;
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
        animator.SetFloat("stamina",stamina);
        animator.SetBool("cast",taunt);
        animator.SetBool("attacking", atking);
        animator.SetBool("dead",dead);
    }

    void Movement()
    {

        int act_num;
        map.TryGetValue(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, out act_num);
        float timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        if (atking) {trail.SetActive(true);trail1.SetActive(true); }
        else { trail.SetActive(false);trail1.SetActive(false); }

        if (stamina > 70 && player_dis >7 ) taunt = true;

        switch (act_num)
        {
            case 0: //Idle
                obstacle.enabled = true;
                agent.enabled = false;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                stamina += 1f;
                if (!atked) choose_atk(act_num);
                break;

            case 1:  //Walk
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/100f;

                if (!atked) choose_atk(act_num);
                break;

            case 2: //Taunt
                obstacle.enabled = true;
                agent.enabled = false;
                taunt = false;

                if(!atked && timer < 0.5) stamina -=50;

                if (timer < 0.5) atked = true;
                else if(timer > 0.8) atked = false;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;

                break;
            case 3:
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.32) {bash = true ;transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.25f * run_sp * Time.deltaTime;}
                else {bash = false ;movement = Vector3.zero;}

                if (timer >= 0.3 && timer <= 0.37) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 4:
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.4){transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Vector3.zero.normalized;}
                else if (timer < 0.63){bash = true;movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;}
                else {bash = false; movement = Vector3.zero;}

                if (timer >= 0.3 && timer <= 0.37) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.48 && timer <= 0.69) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;

            case 5:
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.23) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 0.3f * sp * Time.deltaTime;}
                else if (timer < 0.33){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.51) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.25f * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.48 && timer <= 0.55) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            
            case 6:
                //0.175%~0.27% first atk, 0.35%~0.46% second atk, 0.61%~0.67% thrid atk
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.1) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 0.3f * sp * Time.deltaTime;}
                else if (timer < 0.19){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.49) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.26 && timer <= 0.32) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.48 && timer <= 0.52) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            
            case 7: //Death
                agent.enabled = false;
                transform.rotation = transform.rotation;
                bar.SetActive(false);
                obstacle.enabled = false;
                this.tag = "Dead";
                movement = Vector3.zero;
                break;
        }
    }

    void Start()
    {
        Aim = GameObject.FindGameObjectWithTag("Player");
        p_atking = Aim.GetComponent<ThirdPersonController>().atking;
        atk[0] = "atk0";
        atk[1] = "atk1";
        atk[2] = "atk2";
        atk[3] = "atk3";
        smoothTime = 0.32f;
        for(int i=0;i<atk_n;i++)
            edge.Add(new List<int>());
        
        edge[0].Add(1);
        edge[0].Add(2);
        edge[0].Add(3);
        edge[1].Add(0);
        edge[2].Add(0);
        edge[2].Add(1);
        edge[2].Add(3);
        edge[3].Add(1);
        edge[3].Add(2);
        build();
    }

    // Update is called once per frame
    public override void Update()
    {
        Set_state();
        Healthbar();
        Anime_set();
        Falling();
        Movement();

        controller.Move(gravityMovement + movement);
    }
}
