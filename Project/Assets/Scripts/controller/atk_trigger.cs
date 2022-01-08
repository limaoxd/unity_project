using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atk_trigger : MonoBehaviour
{
    public bool isEnemy = true;
    public bool spell = false;
    public bool atk = false;
    public bool play = false;
    public float Damage = 50;
    public AudioSource audioSource;
    public AudioClip[] audios;
    public int ind = 0;
    public float time = 0f;
    private bool preAtk = false;
    public void OnTriggerEnter(Collider other)
    {
        if (!isEnemy && other.GetComponentInParent<AI>())
        {
            Vector3 point = other.ClosestPoint(transform.position);
            other.GetComponentInParent<AI>().takeDamage(Damage,point);
        }
        else if (isEnemy && other.GetComponentInParent<ThirdPersonController>())
        {
            Vector3 point = other.ClosestPoint(transform.position);
            other.GetComponentInParent<ThirdPersonController>().takeDamage(Damage, point);
        }
        if(spell){
            Object.Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(atk) this.GetComponent<BoxCollider>().enabled = true;
        else this.GetComponent<BoxCollider>().enabled = false;
        
        if(preAtk != atk && atk == true && audios.Length>0) play = true; 
        
        time = audioSource.time;

        if(play){
            ind = Random.Range(0,audios.Length);
            audioSource.PlayOneShot(audios[ind]);
            play = false;
        }
        preAtk = atk;
    }
}
