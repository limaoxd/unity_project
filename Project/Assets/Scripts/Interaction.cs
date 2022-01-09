using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform playerTrans;
    public GameObject interactive_object;
    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    { 
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E))
            {
                interactive_object.SetActive(true);
            }
        }   
    }
}
