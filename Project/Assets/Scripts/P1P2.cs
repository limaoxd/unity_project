using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1P2 : MonoBehaviour
{
    public AI Bearclaw;
    public LevelChanger levelChanger;
    public SaveGame Saver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Bearclaw") && Bearclaw.dead)
        {
            Saver.Save();
            levelChanger.FadeToLevel(3);
        }
    }
}
