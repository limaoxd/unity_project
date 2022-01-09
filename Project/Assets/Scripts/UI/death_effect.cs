using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class death_effect : MonoBehaviour
{
    public GameObject player;
    public GameObject death_text;
    public float smoothTime = 1f;
    public Inventory inventory;
    private float val = 0;
    private Volume volume;
    private ColorAdjustments tmp;
    private ColorAdjustments color;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(volume.profile.TryGet<ColorAdjustments>(out tmp)) color = tmp;

        if(player.GetComponent<ThirdPersonController>().dead) {
            Cursor.visible = true;
            val-=smoothTime*Time.deltaTime;
            death_text.GetComponent<Text>().color = new Color(0.84f,0,0,Mathf.Abs(val/100));
        }
        else {
            if(!inventory.isPause)
                Cursor.visible = false;
            val = 0;
            death_text.GetComponent<Text>().color = new Color(0.84f,0,0,0);
        }
        color.saturation.value = val;
    }
}
