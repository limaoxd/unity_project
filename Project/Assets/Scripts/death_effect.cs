using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class death_effect : MonoBehaviour
{
    private float val = 0;
    private Volume volume;
    private ColorAdjustments tmp;
    private ColorAdjustments color;
    public float smoothTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(volume.profile.TryGet<ColorAdjustments>(out tmp)) color = tmp;

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>().dead) val-=smoothTime*Time.deltaTime;
        else val = 0;

        color.saturation.value = val;
    }
}
