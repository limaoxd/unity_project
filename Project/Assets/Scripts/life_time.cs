using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life_time : MonoBehaviour
{
    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
            Object.Destroy(this.gameObject);
        timer -= Time.deltaTime;
    }
}
