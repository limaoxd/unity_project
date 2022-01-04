using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Bastion : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();
    private List<List<int>> edge = new List<List<int>>();

    void build()
    {
        map.Add(Animator.StringToHash("Walk"),0);
        map.Add(Animator.StringToHash("Taunt"),1);
        map.Add(Animator.StringToHash("Atk0"), 2);
        map.Add(Animator.StringToHash("Atk1"), 3);
        map.Add(Animator.StringToHash("Atk2"), 4);
        map.Add(Animator.StringToHash("Atk3"), 5);
        map.Add(Animator.StringToHash("Dead"), 6);
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 4; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 3.3) return;

        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();

        if (act_num >= 2 && act_num <=5) choosen = edge[act_num - 2][Random.Range(0, edge[act_num - 2].Count)];
        else choosen = Random.Range(0,4);

        if(choosen == 3) Damage = 160;
        else Damage = 125;

        atkTrigger.GetComponent<atk_trigger>().Damage = this.Damage;
        stamina -= 20+Random.Range(-5,5);
        atked = true;
        atk_state[choosen] = true;
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
        animator.SetBool("taunt",taunt);
        animator.SetFloat("stamina", stamina);
        animator.SetBool("attacking", atking);
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

        if (player_dis < 2 && p_atking) taunt = true;

        switch (act_num)
        {
            case 0:
                obstacle.enabled = false;
                agent.enabled = true;
                atking = false;
                targetAngle = transform.rotation.y+agent.angularSpeed;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                agent.speed = sp;
                if(agent.isOnNavMesh) agent.SetDestination(Aim.transform.position);
                movement = agent.velocity/50f;

                if (!atked) choose_atk(act_num);
                break;
            case 1:
                obstacle.enabled = true;
                agent.enabled = false;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                stamina += 10f * Time.deltaTime;
                atking = false;
                atkTrigger.GetComponent<atk_trigger>().atk = false;
                break;

            case 2:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.12) movement = Vector3.zero;
                else if (timer < 0.23) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.36) { movement = Vector3.zero; }
                else if (timer < 0.55) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.12 && timer <= 0.25) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.44 && timer <= 0.56) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer < 0.7) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 3:
                obstacle.enabled = true;
                agent.enabled = false;

                movement = Vector3.zero;

                if (timer >= 0.2 && timer <= 0.4) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer <= 0.3) { atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 4:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.19) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.right.normalized * sp * Time.deltaTime; }
                else if(timer  <0.31) movement = Vector3.zero;
                else if (timer < 0.4) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if(timer  <0.5) movement = Vector3.zero;
                else if (timer < 0.67) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.74) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer >= 0.33 && timer <= 0.4) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else if (timer >= 0.68 && timer <= 0.76) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer <= 0.3) { atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 5:
                obstacle.enabled = true;
                agent.enabled = false;

                if (timer < 0.15) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 1.3f * sp * Time.deltaTime; }
                else if (timer < 0.4) { movement = Vector3.zero; }
                else if (timer < 0.55) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if(timer >= 0.48 && timer <= 0.58) atkTrigger.GetComponent<atk_trigger>().atk = true;
                else atkTrigger.GetComponent<atk_trigger>().atk = false;

                if (timer <= 0.3) {atking = true; atked = false;}
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;

                break;
            case 6:
                agent.enabled = false;
                controller.enabled = false;
                movement = Vector3.zero;
                bar.SetActive(false);
                obstacle.enabled = false;
                atkTrigger.GetComponent<atk_trigger>().atk = false;
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
        maxHealth = 1800;
        Health = 1800;
        smoothTime = 0.7f;

        for(int i=0;i<4;i++)
            edge.Add(new List<int>());

        edge[0].Add(2);
        edge[0].Add(3);
        edge[1].Add(0);
        edge[1].Add(2);
        edge[1].Add(3);
        edge[2].Add(1);
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
