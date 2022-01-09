using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_art : MonoBehaviour
{
    public int ind = 0;
    public RuntimeAnimatorController animController;
    public RuntimeAnimatorController animController1;
    public GameObject[] Weapons;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var it in Weapons){
            if(it == Weapons[ind]) it.SetActive(true);
            else it.SetActive(false);
        }
        if(ind > 8)
            animator.runtimeAnimatorController = animController1 as RuntimeAnimatorController;
        else
            animator.runtimeAnimatorController = animController as RuntimeAnimatorController;
        
    }
}
