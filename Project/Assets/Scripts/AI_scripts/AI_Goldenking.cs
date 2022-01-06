using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Goldenking : AI
{
    public GameObject state2_particle;
    public GameObject trail1;
    public GameObject atkTrigger1;
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Walk"),0);
        map.Add(Animator.StringToHash("Run"),1);
        map.Add(Animator.StringToHash("Taunt"),2);
        map.Add(Animator.StringToHash("Atk0"), 3);
        map.Add(Animator.StringToHash("Atk1"), 4);
        map.Add(Animator.StringToHash("Atk2"), 5);
        map.Add(Animator.StringToHash("Atk3"), 6);
        map.Add(Animator.StringToHash("Atk4"), 7);
        map.Add(Animator.StringToHash("Atk5"), 8);
        map.Add(Animator.StringToHash("Atk6"), 9);
        map.Add(Animator.StringToHash("Grab"), 10);
        map.Add(Animator.StringToHash("Death"), 11);
    }

    void choose(float timer , int act_num){
        if (timer < 0.7) {atking = true; atked = false;}
        if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
        else if (timer >= 0.9) atked = false;
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 7; i++) atk_state[i] = false;
        if (stamina <= 40 || player_dis > 8 ) return;

        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if (!state_2 && act_num >= 3 && act_num <=8) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count-1)];
        else if(!state_2) choosen = Random.Range(0,6);
        else if(state_2 && act_num >= 3 && act_num <=9) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count)];
        else choosen = Random.Range(0,7);

        if(choosen == 0) Damage = 230;
        else if(choosen == 1) Damage = 220;
        else if(choosen == 2) Damage = 170;
        else if(choosen == 3) Damage = 250;
        else if(choosen == 4) Damage = 210;
        else if(choosen == 5) Damage = 200;
        else Damage = 340;
        if(state_2) Damage *= 1.5f;

        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        atkTrigger1.GetComponent<atk_trigger>().Damage = this.Damage;
        stamina -= 15+Random.Range(-10,10);
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
        float timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        map.TryGetValue(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, out act_num);
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);

        if (atking) {
            trail.SetActive(true);
            trail1.SetActive(true);
        }
        else {
            trail.SetActive(false);
            trail1.SetActive(false);
        }

        if(state_2) {
            animator.speed = 1.15f;
            state2_particle.SetActive(true);
        }
        if (player_dis < 3 && p_atking) taunt = true;

        switch (act_num)
        {
            case 0:
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;
                smoothTime = 0.3f;
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/75f;

                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                if (!atked) choose_atk(act_num);
                break;
            case 1:
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;

                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = run_sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/75f;

                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                if (!atked) choose_atk(act_num);
                break;
            case 2:
                obstacle.enabled = true;
                agent.enabled = false;

                movement = Vector3.zero;
                taunt = false;
                stamina += 30f * Time.deltaTime;
                atking = false;
                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                break;
            case 3:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.23) movement = Vector3.zero;
                else if (timer < 0.4) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.3 && timer <= 0.4) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 4:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.28) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Vector3.zero;}
                else if (timer < 0.33) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.25 && timer <= 0.38) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 5:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.12) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * Time.deltaTime; }
                else if (timer < 0.28) { transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Vector3.zero;}
                else if (timer < 0.64) { bash = true ; transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else { bash = false ; movement = Vector3.zero; }

                if (timer >= 0.35 && timer <= 0.6) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 6:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.21) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Vector3.zero;}
                else if (timer < 0.5) { bash = true ; transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else { bash = false ; movement = Vector3.zero; }

                if (timer >= 0.38 && timer <= 0.5) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                
                choose(timer,act_num);
                break;
            case 7:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.26) movement = Vector3.zero;
                else if (timer < 0.5) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.28 && timer <= 0.37) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.5) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 8:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.12) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.18) movement = Vector3.zero;
                else if (timer < 0.26) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.34) movement = Vector3.zero;
                else if (timer < 0.42) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.5) movement = Vector3.zero;
                else if (timer < 0.64) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.18 && timer <= 0.25) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.5) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.62 && timer <= 0.72) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 9:
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.29) movement = Vector3.zero;
                else if (timer < 0.34) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f *sp * Time.deltaTime; }
                else if (timer < 0.38) movement = Vector3.zero;
                else if (timer < 0.43) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f *sp * Time.deltaTime; }
                else if (timer < 0.5) movement = Vector3.zero;
                else if (timer < 0.54) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f *sp * Time.deltaTime; }
                else if (timer < 0.59) movement = Vector3.zero;
                else if (timer < 0.67) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 1.5f *run_sp * Time.deltaTime; }
                else movement = Vector3.zero;
                
                if (timer >= 0.3 && timer <= 0.36) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.45) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.52 && timer <= 0.55) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.64 && timer <= 0.7) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                choose(timer,act_num);
                break;
            case 10:
                obstacle.enabled = true;
                agent.enabled = false;

                //choose(timer,act_num);
                break;
            case 11:
                animator.speed = 0.7f;
                agent.enabled = false;
                controller.enabled = false;
                movement = Vector3.zero;
                bar.SetActive(false);
                obstacle.enabled = false;
                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                this.tag = "Dead";
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
        atk[4] = "atk4";
        atk[5] = "atk5";
        atk[6] = "atk6";
        maxHealth = 3600;
        Health = 3600;
        smoothTime = 0.1f;
        c_dis = 1.3f;

        for(int i=0;i<7;i++)
            edge.Add(new List<int>());

        edge[0].Add(1);
        edge[0].Add(3);
        edge[0].Add(5);
        edge[0].Add(6);
        edge[1].Add(0);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[1].Add(4);
        edge[2].Add(0);
        edge[2].Add(3);
        edge[2].Add(4);
        edge[2].Add(5);
        edge[2].Add(6);
        edge[3].Add(1);
        edge[3].Add(4);
        edge[3].Add(5);
        edge[3].Add(6);
        edge[4].Add(1);
        edge[4].Add(5);
        edge[4].Add(6);
        edge[5].Add(0);
        edge[5].Add(1);
        edge[5].Add(4);
        edge[6].Add(0);
        edge[6].Add(1);
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
