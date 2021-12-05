using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ancient : AI
{
    public Dictionary<int, int> map = new Dictionary<int, int>();

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
        map.Add(Animator.StringToHash("Death"), 8);
    }

    void choose_atk(int act_num)
    {
        atking = false;
        for (int i = 0; i < 4; i++) atk_state[i] = false;
        if (stamina <= 70 || player_dis > 5.5) return;

        int choosen = 0;

        if (act_num == 3)
        { choosen = Random.Range(1, 5); stamina -= 20; }
        else if (act_num == 4)
        { choosen = Random.Range(2, 5); stamina -= 15; }
        else if (act_num == 5)
        { choosen = Random.Range(1, 5); stamina -= 15; }
        else if (act_num == 6)
        { choosen = Random.Range(-1, 2); stamina -= 10; }
        else
        {choosen = Random.Range(0, 4); stamina -= 20; }

        if (choosen > 3 || choosen < 0) return;

        atked = true;
        atk_state[choosen] = true;
    }

    void Set_state()
    {
        float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
        targetAngle = Mathf.Atan2(dis_x,dis_z) * Mathf.Rad2Deg;
        player_dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
        p_atking = Aim.GetComponent<ThirdPersonController>().atking;

        if (stamina >= 100f) { stamina = 100f; }
        else { stamina += 2f * Time.deltaTime; }
    }

    void Anime_set()
    {
        for (int i = 0;i< atk_n;i++)
            animator.SetBool(atk[i], atk_state[i]);
        animator.SetFloat("p_dis",player_dis);
        animator.SetBool("attacking", atking);
        animator.SetBool("dodge", dodge);
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

        if (player_dis < 2 && p_atking) dodge = true;

        switch (act_num)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime;
                if (!atked) choose_atk(act_num);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime;
                if (!atked) choose_atk(act_num);
                break;
            case 2:
                if (timer >= 0.8) dodge = false;

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                if (timer <= 0.5)
                {
                    movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * run_sp * Time.deltaTime;
                    atking = true;
                }
                else { movement = Vector3.zero; stamina += 20f * Time.deltaTime; atking = false; }
                break;

            case 3:
                if (timer < 0.3) movement = Vector3.zero;
                else if (timer < 0.5) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.55) { movement = Vector3.zero; }
                else if (timer < 0.68) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer < 0.7) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 4:
                if (timer < 0.1) movement = Vector3.zero;
                else if (timer < 0.3) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * run_sp * Time.deltaTime; }
                else if (timer < 0.48) { movement = Vector3.zero; }
                else if (timer < 0.64) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 5:
                if (timer < 0.42) movement = Vector3.zero;
                else if (timer < 0.60) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f*run_sp * Time.deltaTime; }
                else movement = Vector3.zero;

                if (timer <= 0.3) { atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 6:
                if (timer < 0.2) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.back.normalized * 1.5f*sp * Time.deltaTime; }
                else if (timer < 0.346) { movement = Vector3.zero; }
                else if (timer < 0.53) { transform.rotation = Quaternion.Euler(0f, angle, 0f); movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward.normalized * 2.0f * run_sp * Time.deltaTime; }
                else { movement = Vector3.zero; }

                if (timer <= 0.3) {atking = true; atked = false; dodge = false; }
                if (!atked && timer >= 0.8 && timer < 0.9) choose_atk(act_num);
                else if (timer >= 0.9) atked = false;
                break;
            case 7:
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                movement = Vector3.zero;
                stamina += 10f * Time.deltaTime;
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
        smoothTime = 0.4f;

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
