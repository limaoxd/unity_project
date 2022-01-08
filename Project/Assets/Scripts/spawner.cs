using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject spawn;
    public bool reset = true;
    private bool lastreset = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(lastreset!=reset && reset){
            Instantiate(spawn, this.transform.position, Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z));
            reset = false;
        }
        lastreset = reset;
    }
}
