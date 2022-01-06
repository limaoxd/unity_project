using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Falcius_Sword : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Idle_Sword"),0);
        map.Add(Animator.StringToHash("Walk"), 1);
        map.Add(Animator.StringToHash("Run"), 2);
        map.Add(Animator.StringToHash("Atk1"), 3);
        map.Add(Animator.StringToHash("Atk2"), 4);
        map.Add(Animator.StringToHash("Atk3"), 5);
        map.Add(Animator.StringToHash("Death"), 6);
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 3; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 3.5) return; //return to idle
        stamina -= 20+Random.Range(-8,1);
        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();


        if(act_num >= 3 && act_num <= 5) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count)];
        else choosen = Random.Range(0,3);

        switch (choosen)
        {
            case 0:
                Damage = 60;
                break;
            case 1:
                Damage = 50;
                break;
            case 2:
                Damage = 70;
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
        animator.SetBool("attacking", atking);
        animator.SetBool("death",dead);
    }

    void Movement()
    {

        int act_num;
        map.TryGetValue(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, out act_num);
        float timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        if (atking) {trail.SetActive(true); }
        else { trail.SetActive(false); }

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

            case 3: //Atk1
                obstacle.enabled = true;
                agent.enabled = false;
                if (timer < 0.4)
                { 
                    movement = Vector3.zero;
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
                else if (timer < 0.7) 
                    movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                else 
                    movement = Vector3.zero;

                if (timer >= 0.45 && timer <= 0.54) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer < 0.7) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;
            case 4: //Atk2  triple slash
                obstacle.enabled = true;
                agent.enabled = false;
                if (timer < 0.16) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * 0.3f * Time.deltaTime; }
                else if (timer < 0.27) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.35) movement = Vector3.zero;
                else if (timer < 0.45) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.63) movement = Vector3.zero;
                else if (timer < 0.7) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.14 && timer <= 0.3) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.4 && timer <= 0.55) atkTrigger.GetComponent<atk_trigger>().atk =  true;
                else if (timer >= 0.63 && timer <= 0.65) atkTrigger.GetComponent<atk_trigger>().atk =  true;
                else atkTrigger.GetComponent<atk_trigger>().atk =  false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;

            case 5:
                //0.175%~0.27% first atk, 0.35%~0.46% second atk, 0.61%~0.67% thrid atk
                obstacle.enabled = true;
                agent.enabled = false;
                if (timer < 0.3) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * 0.3f * Time.deltaTime; }
                else if(timer < 0.48) movement = Vector3.zero;
                else if (timer < 0.63) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.5 && timer <= 0.56) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;

            case 6: //Death
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
        atk[0] = "atk1";
        atk[1] = "atk2";
        atk[2] = "atk3";
        maxHealth = 350;
        Health = 350;
        smoothTime = 0.4f;
        for(int i=0;i<3;i++)
            edge.Add(new List<int>());
        
        edge[0].Add(1); //1 -> 2
        edge[0].Add(2); //1 -> 3
        edge[1].Add(2); //2 -> 3
        edge[2].Add(1); //3 -> 2
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
