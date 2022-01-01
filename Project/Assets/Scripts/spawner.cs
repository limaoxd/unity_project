using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject spawn;
    public bool reset = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(reset){
            Instantiate(spawn, this.transform.position, Quaternion.Euler(0,transform.eulerAngles.y,0));
            reset = false;
        }
    }
}
