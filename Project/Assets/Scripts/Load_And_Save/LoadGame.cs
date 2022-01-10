using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    string JsonString;
    JsonData allData;
    public ThirdPersonController Player;
    public Text HP;
    public Text ATK;
    public Text STAMINA;
    public Text LEVEL;
    public Text SKILL;
    public Text EXP;
    public Transform SP;

    // Start is called before the first frame update
    void Awake()
    {
        JsonString = File.ReadAllText(Application.dataPath + @"/StreamingAssets/archive.json");
        allData = JsonMapper.ToObject(JsonString);
        int number = allData.Count;

        for (int i = 0; i < allData.Count; i++)
        {
            if (allData[i]["name"].ToString() == "HP")
            {
                HP.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "ATK")
            { 
                ATK.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "Stamina")
            { 
                STAMINA.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "Level")
            { 
                LEVEL.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "Skill")
            { 
                SKILL.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "RequiredEXP")
            { 
                EXP.text = (int)allData[i]["Level"] + "";
                number--;
            }
            if (allData[i]["name"].ToString() == "Scene")
            {
                
            }
        }
    }

    public int CheckScene()
    {
        for (int i = 0; i < allData.Count; i++)
        {
            if (allData[i]["name"].ToString() == "Scene")
            {
                return (int)allData[i]["scene"];
            }
        }
        return 0;
    }
}
