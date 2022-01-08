using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell : MonoBehaviour
{
    public float sp = 5;
    public float smoothTime = 0.01f;
    public GameObject explode;
    private Transform Aim;
    private Vector3 angle;
    private Vector3 targetAngle;

    private void OnTriggerEnter(Collider other) {
        Object.Destroy(this.gameObject);
        Instantiate(explode, this.transform.position, Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z));
    }

    // Start is called before the first frame update
    void Start()
    {
        Aim = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetAngle = Aim.position - transform.position;
        Quaternion angle = Quaternion.LookRotation(targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, angle, smoothTime);
        transform.position += transform.rotation * Vector3.forward.normalized * sp * Time.deltaTime;
    }
}
