using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Falcius_Elite : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    public GameObject atkTrigger1;

    void build()
    {
        map.Add(Animator.StringToHash("Idle"),0);
        map.Add(Animator.StringToHash("Walk"), 1);
        map.Add(Animator.StringToHash("Run"), 2);
        map.Add(Animator.StringToHash("Taunt"), 3);
        map.Add(Animator.StringToHash("Atk0"), 4);
        map.Add(Animator.StringToHash("Atk1"), 5);
        map.Add(Animator.StringToHash("Atk2"), 6);
        map.Add(Animator.StringToHash("Atk3"), 7);
        map.Add(Animator.StringToHash("Death"), 8);
    }

    void choose(float timer , int act_num){
        if (timer < 0.7) {atking = true; atked = false;}
        if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
        else if (timer >= 0.9) atked = false;
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 4; i++) atk_state[i] = false;
        if (stamina <= 50 || player_dis > 4) return; //return to idle
        stamina -= 15+Random.Range(-5,5);
        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if(act_num >= 4 && act_num <= 7) choosen = edge[act_num - 4][Random.Range(0, edge[act_num - 4].Count)];
        else choosen = Random.Range(0,4);

        switch (choosen)
        {
            case 0:
                Damage = 110;
                break;
            case 1:
                Damage = 100;
                break;
            case 2:
                Damage = 150;
                break;
            case 3:
                Damage = 115;
                break;
        }
        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        atked = true;
        atk_state[choosen] = true;
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
        animator.SetFloat("stamina",stamina);
        animator.SetBool("taunt",taunt);
        animator.SetBool("attacking", atking);
        animator.SetBool("dead",dead);
    }

    void Movement()
    {

        int act_num;
        map.TryGetValue(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, out act_num);
        float timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        if (atking) {trail.SetActive(true); }
        else { trail.SetActive(false); }

        if (player_dis < 3 && p_atking) taunt = true;

        switch (act_num)
        {
            case 0: //Idle
                obstacle.enabled = true;
                agent.enabled = false;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
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

            case 2: //Run
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = run_sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/100f;

                if (!atked) choose_atk(act_num);
                break;
            case 3: //Taunt
                obstacle.enabled = true;
                agent.enabled = false;
                taunt = false;

                stamina += 15f * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                break;
            case 4: //Atk1
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.1){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.204) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * Time.deltaTime;}
                else if (timer < 0.34){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.45) {bash = true; transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2f * run_sp * Time.deltaTime;}
                else {bash = false;movement = Vector3.zero;}

                if (timer >= 0.4 && timer <= 0.48) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 5: //Atk2  triple slash
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.08){transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * Time.deltaTime;}
                else if (timer < 0.26){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.3) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;}
                else if (timer < 0.41){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.63) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.26 && timer <= 0.34) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.42 && timer <= 0.53) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.58 && timer <= 0.7) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;

            case 6:
                //0.175%~0.27% first atk, 0.35%~0.46% second atk, 0.61%~0.67% thrid atk
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.16){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.24) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;}
                else if (timer < 0.38) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.12 && timer <= 0.23) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.3 && timer <= 0.44) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            
            case 7:
                //0.175%~0.27% first atk, 0.35%~0.46% second atk, 0.61%~0.67% thrid atk
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.17){transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.left.normalized * sp * Time.deltaTime;}
                else if (timer < 0.27) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;}
                else if (timer < 0.38){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.43) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.2f * run_sp * Time.deltaTime;}
                else if (timer < 0.6){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.7) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.22 && timer <= 0.28) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.46) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.6 && timer <= 0.65) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.66 && timer <= 0.73) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            
            case 8: //Death
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
        maxHealth = 800;
        Health = 800;
        smoothTime = 0.3f;
        for(int i=0;i<atk_n;i++)
            edge.Add(new List<int>());
        
        edge[0].Add(2);
        edge[0].Add(3);
        edge[1].Add(0);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[2].Add(0);
        edge[2].Add(1);
        edge[2].Add(3);
        edge[3].Add(0);
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
