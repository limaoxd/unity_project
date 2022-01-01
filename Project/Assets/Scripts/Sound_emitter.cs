using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_emitter : MonoBehaviour
{
    public bool run = false;
    private bool last = false;
    public AudioSource audioSource;
    public AudioClip[] audios;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(last != run && run) audioSource.PlayOneShot(audios[Random.Range(0,audios.Length)]);
        last = run;
    }
}
