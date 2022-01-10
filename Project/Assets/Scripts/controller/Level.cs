using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Button hpButton;
    public Button atkButton;
    public Button staminaButton;
    public Text level;
    public Text Required;
    public Text hpLevel;
    public Text ATKLevel;
    public Text StaminaLevel;
    public Text hpStat;
    public Text ATKStat;
    public Text StaminaStat;
    public Text PointLeft;
    public int mobexp;
    private ThirdPersonController player;
    private List<int> req = new List<int>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
        for(int i=1;i<=29;i++)
        {
            if (i >= 0 && i <= 7)
                req.Add(150 * i * i + 1050 * i);
            else if(i >= 8 && i <= 11)
                req.Add(200 * i * i + 1050 * i - 2450);
            else if (i >= 12 && i <= 22)
                req.Add(500 * i * i + 1750 * i + 9800);
            else if (i >= 23 && i <= 29)
                req.Add(250 * i * i - 1500 * i - 22750);
        }
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

    public void LevelSearch()
    {
        hpLevel.text = (Int32.Parse(hpLevel.text)).ToString();
        hpStat.text = (Math.Round(400 * (0.15 * Int32.Parse(hpLevel.text) + 1))).ToString();
        player.maxHealth = Int32.Parse(hpStat.text);

        ATKLevel.text = (Int32.Parse(ATKLevel.text)).ToString();
        ATKStat.text = (1 + (0.1 * Int32.Parse(ATKLevel.text))).ToString();
        StaminaLevel.text = (Int32.Parse(StaminaLevel.text)).ToString();

        StaminaStat.text = (Math.Round(150 * (0.15 * Int32.Parse(StaminaLevel.text) + 1))).ToString();
        player.maxStamina = Int32.Parse(StaminaStat.text);
    }

    public void PlayerLevelUp()
    {
        level.text = (Int32.Parse(level.text) + 1).ToString();
        PointLeft.text = (Int32.Parse(PointLeft.text) + 1).ToString();
        Required.text = (req[Int32.Parse(level.text)]).ToString();
    }

    private void Update()
    {
        if(Int32.Parse(PointLeft.text) == 0)
        {
            hpButton.gameObject.SetActive(false);
            atkButton.gameObject.SetActive(false);
            staminaButton.gameObject.SetActive(false);
        }
        else
        {
            hpButton.gameObject.SetActive(true);
            atkButton.gameObject.SetActive(true);
            staminaButton.gameObject.SetActive(true);
        }
        while(mobexp != 0)
        {
            if(mobexp >= Int32.Parse(Required.text))
            {
                mobexp -= Int32.Parse(Required.text);
                PlayerLevelUp();
            }
            else
            {
                Required.text = (Int32.Parse(Required.text) - mobexp).ToString();
                mobexp = 0;
            }
        }
    }
}