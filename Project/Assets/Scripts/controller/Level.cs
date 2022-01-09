using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Text hpLevel;
    public Text ATKLevel;
    public Text StaminaLevel;
    public Text hpStat;
    public Text ATKStat;
    public Text StaminaStat;
    private ThirdPersonController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    public void LevelUp(int index)
    {
        if (index == 1) //HP button
        {
            hpLevel.text = (Int32.Parse(hpLevel.text) + 1).ToString();
            hpStat.text = (Math.Round(400 * (0.15 * Int32.Parse(hpLevel.text) + 1))).ToString();
            player.maxHealth = Int32.Parse(hpStat.text);
        }
        else if (index == 2) //ATK button
        {
            ATKLevel.text = (Int32.Parse(ATKLevel.text) + 1).ToString();
            ATKStat.text = (1 + (0.1 * Int32.Parse(ATKLevel.text))).ToString();
        }
        else if (index == 3) //Stamina button
        {
            StaminaLevel.text = (Int32.Parse(StaminaLevel.text) + 1).ToString();
            StaminaStat.text = (Math.Round(150 * (0.15 * Int32.Parse(StaminaLevel.text) + 1))).ToString();
            player.maxStamina = Int32.Parse(StaminaStat.text);
        }
    }
}
