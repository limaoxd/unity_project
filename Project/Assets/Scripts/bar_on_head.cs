using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar_on_head : MonoBehaviour
{
    public GameObject target;
    void SetBarPos(){
        this.transform.position = Camera.main.WorldToScreenPoint(target.transform.position + new Vector3(0,0.4f,0));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBarPos();
    }
}
