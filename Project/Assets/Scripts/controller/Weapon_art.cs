using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_art : MonoBehaviour
{
    public int ind = 0;
    public RuntimeAnimatorController animController;
    public RuntimeAnimatorController animController1;
    public GameObject[] Weapons;

    private int preInd = -1;
    private Animator animator;

    private GameObject myWeapon;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ind != preInd){
            if(myWeapon) Object.Destroy(this.myWeapon);
            myWeapon = Instantiate(Weapons[ind], this.transform.position , transform.rotation);
            myWeapon.transform.parent = this.transform;
            myWeapon.transform.localPosition = new Vector3(0,0,0);
            myWeapon.transform.localRotation =  Quaternion.Euler(0,90,90);
            myWeapon.transform.localScale = new Vector3(1,1,1);
        }
        
        if(ind > 7)
            animator.runtimeAnimatorController = animController1 as RuntimeAnimatorController;
        else
            animator.runtimeAnimatorController = animController as RuntimeAnimatorController;
        
        preInd = ind;
    }
}
