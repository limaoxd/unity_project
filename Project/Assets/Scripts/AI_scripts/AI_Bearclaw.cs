using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Bearclaw : AI
{
    public GameObject state2_particle;
    public GameObject trail1;
    public GameObject atkTrigger1;
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Idle"),0);
        map.Add(Animator.StringToHash("Walk"),1);
        map.Add(Animator.StringToHash("Run"),2);
        map.Add(Animator.StringToHash("Taunt"),3);
        map.Add(Animator.StringToHash("Atk0"), 4);
        map.Add(Animator.StringToHash("Atk1"), 5);
        map.Add(Animator.StringToHash("Atk2"), 6);
        map.Add(Animator.StringToHash("Atk3"), 7);
        map.Add(Animator.StringToHash("Atk_F"), 8);
        map.Add(Animator.StringToHash("Show"), 9);
        map.Add(Animator.StringToHash("Death"), 10);
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 5; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 6 ) return;

        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if (!state_2 && act_num >= 4 && act_num <=6) choosen = edge[act_num - 4][Random.Range(0, edge[act_num - 4].Count-1)];
        else if(state_2 && act_num >= 4 && act_num <=8) choosen = edge[act_num - 4][Random.Range(0, edge[act_num - 4].Count)];
        else if(!state_2) choosen = Random.Range(0,4);
        else choosen = Random.Range(0,5);

        if(choosen == 3) Damage = 90;
        else if(choosen == 4) Damage = 100;
        else  Damage = 80;

        if(state_2) Damage *= 1.25f;

        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        atkTrigger1.GetComponent<atk_trigger>().Damage = this.Damage;
        stamina -= 15+Random.Range(-2,10);
        atked = true;
        atk_state[choosen] = true;
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
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
                movement = Vector3.zero;

                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                if (!atked) choose_atk(act_num);
                break;
            case 1:
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
            case 2:
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;
                smoothTime = 0.3f;
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = run_sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/75f;

                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                if (!atked) choose_atk(act_num);
                break;
            case 3:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                taunt = false;
                stamina += 20f * Time.deltaTime;
                atking = false;
                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                break;
            case 4:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                if (timer < 0.21) movement = Vector3.zero;
                else if (timer < 0.42) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.5) { movement = Vector3.zero; }
                else if (timer < 0.75) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.27 && timer <= 0.4) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.52 && timer <= 0.63) atkTrigger1.GetComponent<atk_trigger>().atk = true;
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                if (timer < 0.7) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 5:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                if (timer < 0.22) movement = Vector3.zero;
                else if (timer < 0.34) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.46) movement = Vector3.zero;
                else if (timer < 0.56) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.63) movement = Vector3.zero;
                else if (timer < 0.72) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;


                if (timer >= 0.05 && timer <= 0.1) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                else if (timer >= 0.27 && timer <= 0.35) {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                else if (timer >= 0.5 && timer <= 0.57) {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else if (timer >= 0.65 && timer <= 0.7) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                if (timer < 0.7) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 6:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                if(timer  <0.31) movement = Vector3.zero;
                else if (timer < 0.45) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.4 && timer <= 0.5) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                if (timer <= 0.3) { atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 7:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                if (timer < 0.25) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 0.5f * sp * Time.deltaTime; }
                else if (timer < 0.3) movement = Vector3.zero;
                else if (timer < 0.66) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if(timer >= 0.4 && timer <= 0.5) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = false;}
                else if(timer >= 0.57 && timer <= 0.66) {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                if (timer <= 0.3) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;

                break;
            case 8:
                smoothTime = 0.25f;
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.2) {transform.rotation = Quaternion.Euler(0f, angle, 0f);movement = Vector3.zero;}
                else if (timer < 0.55) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * 1.2f * run_sp * Time.deltaTime; }
                else if (timer < 0.65) movement = Vector3.zero;
                else if (timer < 0.8) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2f*sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if(timer >= 0.3 && timer <= 0.54) {atkTrigger.GetComponent<atk_trigger>().atk = true;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else if(timer >= 0.62 && timer <= 0.73) {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = true;}
                else {atkTrigger.GetComponent<atk_trigger>().atk = false;atkTrigger1.GetComponent<atk_trigger>().atk = false;}

                if (timer <= 0.3) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;

                break;
            case 9:
                obstacle.enabled = true;
                agent.enabled = false;
                smoothTime = 0.3f;

                if (timer < 0.43) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 0.5f * sp * Time.deltaTime; }
                else movement = Vector3.zero;

                atkTrigger.GetComponent<atk_trigger>().atk = false;
                atkTrigger1.GetComponent<atk_trigger>().atk = false;
                if (!atked) choose_atk(act_num);
                break;
            case 10:
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
        atk[4] = "atk_f";
        maxHealth = 2000;
        Health = 2000;
        smoothTime = 0.3f;
        c_dis = 1.15f;

        for(int i=0;i<5;i++)
            edge.Add(new List<int>());

        edge[0].Add(2);
        edge[0].Add(4);
        edge[1].Add(0);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[1].Add(4);
        edge[2].Add(3);
        edge[2].Add(4);
        edge[3].Add(4);
        edge[4].Add(0);
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
