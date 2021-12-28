using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ancient : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Walk"),0);
        map.Add(Animator.StringToHash("Run"), 1);
        map.Add(Animator.StringToHash("Dodge"), 2);
        map.Add(Animator.StringToHash("Atk1"), 3);
        map.Add(Animator.StringToHash("Atk2"), 4);
        map.Add(Animator.StringToHash("Atk3"), 5);
        map.Add(Animator.StringToHash("Atk4"), 6);
        map.Add(Animator.StringToHash("Hand_gesture"), 7);
        map.Add(Animator.StringToHash("Tired"), 8);
        map.Add(Animator.StringToHash("Death"), 9);
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 4; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 5.5) return;

        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if (act_num >= 3 && act_num <=6) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count)];
        else choosen = Random.Range(0,4);

        switch (choosen)
        {
            case 0:
                Damage = 140;
                break;
            case 1:
                Damage = 150;
                break;
            case 2:
                Damage = 200;
                break;
            case 3:
                Damage = 250;
                break;
        }
        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        stamina -= 20+Random.Range(-8,1);
        atked = true;
        atk_state[choosen] = true;
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
        animator.SetFloat("stamina", stamina);
        animator.SetFloat("poise", poise);
        animator.SetBool("attacking", atking);
        animator.SetBool("dodge", dodge);
        animator.SetBool("death",dead);
    }

    void Movement()
    {

        int act_num;
        float timer = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        map.TryGetValue(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, out act_num);
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);

        if (atking) trail.SetActive(true);
        else trail.SetActive(false);

        if (player_dis < 2 && p_atking) dodge = true;

        switch (act_num)
        {
            case 0:
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = sp/100;
                agent.enabled = true;
                agent.SetDestination(Aim.transform.position);
                movement = agent.velocity;

                if (!atked) choose_atk(act_num);
                break;
            case 1:
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = run_sp / 100;
                agent.enabled = true;
                agent.SetDestination(Aim.transform.position);
                movement = agent.velocity;

                if (!atked) choose_atk(act_num);
                break;
            case 2:
                Damage = 100;
                if (timer >= 0.8) dodge = false;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                if (timer <= 0.5)
                {
                    atkTrigger.GetComponent<BoxCollider>().enabled = true;
                    movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
                    atking = true;
                }
                else { movement = Vector3.zero; stamina += 20f * Time.deltaTime; atking = false; atkTrigger.GetComponent<BoxCollider>().enabled = false; }
                break;

            case 3:
                if (timer < 0.3) movement = Vector3.zero;
                else if (timer < 0.5) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.55) { movement = Vector3.zero; }
                else if (timer < 0.68) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.35 && timer <= 0.45) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else if (timer >= 0.58 && timer <= 0.68) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer < 0.7) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 4:
                if (timer < 0.1) movement = Vector3.zero;
                else if (timer < 0.3) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.48) { movement = Vector3.zero; }
                else if (timer < 0.64) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.28 && timer <= 0.35) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else if (timer >= 0.55 && timer <= 0.65) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 5:
                if (timer < 0.42) movement = Vector3.zero;
                else if (timer < 0.60) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f*run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.55 && timer <= 0.6) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 6:
                if (timer < 0.2) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 1.5f*sp * Time.deltaTime; }
                else if (timer < 0.346) { movement = Vector3.zero; }
                else if (timer < 0.53) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if(timer >= 0.47 && timer <= 0.55) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer <= 0.3) {atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;

                break;
            case 7:
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                stamina += 15f * Time.deltaTime;
                break;
            case 8:
                movement = Vector3.zero;
                poise = 100;
                break;
            case 9:
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
        atk[3] = "atk4";
        maxHealth = 2000;
        Health = 2000;
        smoothTime = 0.4f;

        for(int i=0;i<4;i++){
            edge.Add(new List<int>());
        }

        edge[0].Add(1);
        edge[0].Add(2);
        edge[0].Add(3);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[2].Add(1);
        edge[2].Add(3);
        edge[3].Add(0);
        edge[3].Add(1);
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
