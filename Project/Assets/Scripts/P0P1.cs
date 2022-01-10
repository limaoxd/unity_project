using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P0P1 : MonoBehaviour
{
    public GameObject ancient;
    public ThirdPersonController player;
    public LevelChanger levelChanger;
    public SaveGame Saver;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Ancient_Glaive(Clone)") && player.dead)
        {
            Saver.Save();
            levelChanger.FadeToLevel(2);
        }
    }
}
