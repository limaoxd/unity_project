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
    public Level level;

    // Start is called before the first frame update
    public void Load()
    {
        try
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
                    ;
                }
            }
        }
        catch
        {
            ;
        }
    }

    public int CheckScene()
    {
        string String = File.ReadAllText(Application.dataPath + @"/StreamingAssets/archive.json");
        JsonData Data = JsonMapper.ToObject(String);
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i]["name"].ToString() == "Scene")
            {
                return (int)Data[i]["Scene"];
            }
        }
        return 0;
    }
}
