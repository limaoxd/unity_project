using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atk_trigger : MonoBehaviour
{
    public bool isEnemy = true;
    public float Damage = 50;
    private int t = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (!isEnemy && other.GetComponentInParent<AI>())
        {
            other.GetComponentInParent<AI>().takeDamage(Damage);
        }
        else if (isEnemy && other.GetComponentInParent<ThirdPersonController>())
        {
            other.GetComponentInParent<ThirdPersonController>().takeDamage(Damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
