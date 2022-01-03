using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Restart_button : MonoBehaviour
{
    public GameObject player;
    public Button button;

    void OnClick(){
        player.GetComponent<ThirdPersonController>().Reset_state();
        foreach(var it in GameObject.FindGameObjectsWithTag("Enemy"))
            Object.Destroy(it);
        foreach(var it in GameObject.FindGameObjectsWithTag("Dead"))
            Object.Destroy(it);
        foreach(var it in GameObject.FindGameObjectsWithTag("Respawn"))
            it.GetComponent<spawner>().reset = true;
        
        GameObject boss_gate = GameObject.Find("boss_gate");
        foreach(var it in boss_gate.GetComponents<BoxCollider>())
            it.enabled = !it.enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<RectTransform>().sizeDelta.x == 0 && player.GetComponent<ThirdPersonController>().dead) this.GetComponent<RectTransform>().sizeDelta = new Vector2(320 , 60);
        else if(!player.GetComponent<ThirdPersonController>().dead)this.GetComponent<RectTransform>().sizeDelta = new Vector2(0 , 0);
    }
}
