using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Decimus : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Walk"), 0);
        map.Add(Animator.StringToHash("Taunt"), 1);
        map.Add(Animator.StringToHash("Atk0"), 2);
        map.Add(Animator.StringToHash("Atk1"), 3);
        map.Add(Animator.StringToHash("Atk2"), 4);
        map.Add(Animator.StringToHash("Atk3"), 5);
        map.Add(Animator.StringToHash("Death"), 6);
    }

    void choose(float timer , int act_num){
        if (timer < 0.7) {atking = true; atked = false;}
        if (!atked && timer >= 0.7 && timer < 0.75) choose_atk(act_num);
        else if (timer >= 0.9) atked = false;
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < atk_n; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 4.5) return; //return to idle
        stamina -= 20+Random.Range(-1,5);
        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if(act_num >= 2 && act_num <= 5) choosen = edge[act_num - 2][Random.Range(0, edge[act_num - 2].Count)];
        else choosen = Random.Range(0,4);

        switch (choosen)
        {
            case 0:
                Damage = 150;
                break;
            case 1:
                Damage = 150;
                break;
            case 2:
                Damage = 200;
                break;
            case 3:
                Damage = 180;
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
        if (atking) trail.SetActive(true);
        else trail.SetActive(false);

        if (stamina < 70 && p_atking ) taunt = true;

        switch (act_num)
        {
            case 0:  //Walk
                animator.speed = 1f;
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

            case 1: //Taunt
                animator.speed = 1f;
                obstacle.enabled = true;
                agent.enabled = false;
                taunt = false;

                stamina +=5f;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;

                break;
            case 2:
                animator.speed = 0.7f;
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.6) {movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.62) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.15 && timer <= 0.2) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.61 && timer <= 0.66) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                choose(timer,act_num);
                break;
            case 3:
                animator.speed = 0.6f;
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.15) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;}
                else if (timer < 0.39){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.61) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.21 && timer <= 0.32) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.48 && timer <= 0.55) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                choose(timer,act_num);
                break;

            case 4:
                animator.speed = 0.8f;
                obstacle.enabled = true;
                agent.enabled = false;
                
                if (timer < 0.42){movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.51) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else movement = Vector3.zero;

                if (timer >= 0.49 && timer <= 0.53) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                choose(timer,act_num);
                break;
            
            case 5:
                animator.speed = 0.7f;
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.18) {movement = Vector3.zero;transform.rotation = Quaternion.Euler(0f, angle, 0f);}
                else if (timer < 0.22)movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                if (timer < 0.24) movement = Vector3.zero;
                else if (timer < 0.44) {bash = true ; transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;}
                else {bash = false ; movement = Vector3.zero;}

                if (timer >= 0.15 && timer <= 0.25) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.26 && timer <= 0.32) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.33 && timer <= 0.39) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.42) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.43 && timer <= 0.5) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                choose(timer,act_num);
                break;
            
            case 6: //Death
                animator.speed = 1f;
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
        smoothTime = 0.55f;
        for(int i=0;i<atk_n;i++)
            edge.Add(new List<int>());
        
        edge[0].Add(1);
        edge[0].Add(3);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[2].Add(0);
        edge[2].Add(1);
        edge[2].Add(3);
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
