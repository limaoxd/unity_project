using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_gate : MonoBehaviour
{
    public GameObject[] boss_spawner;
    public string[] BossName;
    private GameObject[] boss;
    public void OnTriggerExit(Collider other)
    {
        foreach(var it in GetComponents<BoxCollider>())
            it.enabled = !it.enabled;
        foreach(var it in boss_spawner)
            it.GetComponent<spawner>().reset = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        boss = new GameObject[BossName.Length];
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;

        for(int i = 0 ;i < BossName.Length ; i++){
            if(!boss[i] && GameObject.Find(BossName[i])) boss[i] = GameObject.Find(BossName[i]);
            if(boss[i] && boss[i].GetComponent<AI>().dead) count ++;
        }
    
        if(count == BossName.Length)  Object.Destroy(this.gameObject);
    }
}
