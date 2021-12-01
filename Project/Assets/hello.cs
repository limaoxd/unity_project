using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hello : MonoBehaviour
{
    public Vector3 movingDic;
    [SerializeField] float sp = 10f;

    void Awake(){
        Debug.Log("awake");
    } 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        Transform t = GetComponent<Transform>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
 
        t.position = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("update");
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += sp * Time.deltaTime * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += sp * Time.deltaTime * Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += sp * Time.deltaTime * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += sp * Time.deltaTime * Vector3.right;
        }
    }
}
