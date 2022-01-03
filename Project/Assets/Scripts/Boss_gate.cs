using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_gate : MonoBehaviour
{
    public void OnTriggerExit(Collider other)
    {
        foreach(var it in GetComponents<BoxCollider>())
            it.enabled = !it.enabled;
        GameObject.Find("Boss_Spawner").GetComponent<spawner>().reset = true;
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
