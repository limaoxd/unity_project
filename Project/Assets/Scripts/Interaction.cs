using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    //public Transform playerTrans;
    public GameObject isactive_object;
    public GameObject rotate_object;
    public GameObject movement_object;
    public Quaternion rotate_angle;
    private Quaternion cur_angle;
    // Start is called before the first frame update
    void Start()
    {
        cur_angle = this.transform.rotation;
    }
    // Update is called once per frame
    void Update()
    { 
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, cur_angle, 0.03f);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E))
            {
                if(isactive_object!=null){isactive_object.SetActive(true);}
                if(rotate_object!=null){cur_angle = rotate_angle;}//rotate the target
                Debug.Log("123");
            }
        }   
    }
}
