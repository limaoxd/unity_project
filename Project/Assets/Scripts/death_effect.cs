using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class death_effect : MonoBehaviour
{
    public GameObject death_text;
    public float smoothTime = 1f;
    private float val = 0;
    private Volume volume;
    private ColorAdjustments tmp;
    private ColorAdjustments color;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(volume.profile.TryGet<ColorAdjustments>(out tmp)) color = tmp;

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>().dead) {
            val-=smoothTime*Time.deltaTime;
            death_text.GetComponent<Text>().color = new Color(0.84f,0,0,Mathf.Abs(val/100));
        }
        else {
            val = 0;
            death_text.GetComponent<Text>().color = new Color(0.84f,0,0,0);
        }
        color.saturation.value = val;
    }
}
