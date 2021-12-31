using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Warrior_Sword : AI
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
        Debug.Log(Animator.StringToHash("Walk"));
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 3; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 2) return; //return to idle
        stamina -= 20+Random.Range(-8,1);
        int choosen = 0;

        List<AnimatorStateInfo> path = new List<AnimatorStateInfo>();


        if(act_num >= 3 && act_num <= 5) choosen = edge[act_num - 3][Random.Range(0, edge[act_num - 3].Count)];
        else choosen = Random.Range(0,3);

        switch (choosen)
        {
            case 0:
                Damage = 35;
                break;
            case 1:
                Damage = 35;
                break;
            case 2:
                Damage = 35;
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
        animator.SetFloat("player_dis",player_dis);
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
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                if (!atked) choose_atk(act_num);
                break;

            case 1:  //Walk
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
                if (!atked) choose_atk(act_num);
                break;

            case 2: //Run
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                if (!atked) choose_atk(act_num);
                break;

            case 3: //Atk1
                if (timer < 0.4)
                { 
                    movement = Vector3.zero;
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
                else if (timer < 0.5) 
                    movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
                else 
                    movement = Vector3.zero;

                if (timer >= 0.45 && timer <= 0.65) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer < 0.7) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;
            case 4: //Atk2
                if (timer < 0.1) movement = Vector3.zero;
                else if (timer < 0.25) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * sp * Time.deltaTime; }
                else if (timer < 0.35) movement = Vector3.zero;
                else if (timer < 0.45) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer >= 0.28 && timer <= 0.35) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else if (timer >= 0.55 && timer <= 0.65) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;

            case 5: //Atk3  triple slash
            //0.175%~0.27% first atk, 0.35%~0.46% second atk, 0.61%~0.67% thrid atk
                if (timer < 0.1) movement = Vector3.zero;
                else if (timer < 0.175) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.27) movement = Vector3.zero;
                else if (timer < 0.35) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.46) movement = Vector3.zero; 
                else if (timer < 0.61) {transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else if (timer < 0.67) movement = Vector3.zero; 
                else movement = Vector3.zero; 

                if (timer >= 0.175 && timer <= 0.27) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else if (timer >= 0.4 && timer <= 0.46) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else if (timer >= 0.61 && timer <= 0.67) atkTrigger.GetComponent<BoxCollider>().enabled = true;
                else atkTrigger.GetComponent<BoxCollider>().enabled = false;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; } //finish attacking animation
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num); //choose next attack
                else if (timer >= 0.9) atked = false; //finish whole attack
                break;

            case 6: //Death
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
        maxHealth = 250;
        Health = 250;
        smoothTime = 0.4f;
        for(int i=0;i<3;i++){
            edge.Add(new List<int>());
        }

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
        Anime_set();
        Falling();
        Movement();

        controller.Move(gravityMovement + movement);
    }
}
