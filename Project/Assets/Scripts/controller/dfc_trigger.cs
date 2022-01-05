using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dfc_trigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audios;
    public int ind = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<AI>() && other.GetComponentInParent<ThirdPersonController>().atking){
            this.GetComponentInParent<AI>().takeDfc();
            ind = Random.Range(0,audios.Length);
            audioSource.PlayOneShot(audios[ind]);
        }
    }
    public void OnTriggerExit(Collider other) {
        this.GetComponentInParent<AI>().noDfc();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
