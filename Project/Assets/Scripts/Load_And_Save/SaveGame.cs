using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    public Transform PlayerLocation;
    public Inventory CharacterInventory;
    public ThirdPersonController Player;
    public Text HPPoint;
    public Text ATKPoint;
    public Text StaminaPoint;
    public Text LevelPoint;
    public Text SkillPoint;
    public Text EXPPoint;
    FileInfo Finfo;
    StreamWriter SW;

    // Start is called before the first frame update
    void Start()
    {
        Finfo = new FileInfo(Application.dataPath + @"/StreamingAssets/archive.json");
        SW = Finfo.CreateText();
        SW.WriteLine("[");
    }

    public void Save()
    {
        SaveItems();
        SaveStats();
        SW.Write("]");
        SW.Close();
    }

    public void SaveItems()
    {
        //save weapon
        JsonData ItemData;
        ItemSave item;
        List<Item> Items = CharacterInventory.characterItems;

        for (int i = 0; i < Items.Count; i++)
        {
            item = new ItemSave(Items[i]);
            ItemData = JsonMapper.ToJson(item);
            SW.Write(ItemData.ToString());
            SW.WriteLine(",");
        }

        //save max_potion
        PotionSave potion = new PotionSave(Player.drinkMax);
        JsonData potionData = JsonMapper.ToJson(potion);
        SW.Write(potionData.ToString());
        SW.WriteLine(",");
    }

    public void SaveStats()
    {
        //save level
        StatsSave stats = new StatsSave("HP", int.Parse(HPPoint.text));
        JsonData statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");
        stats = new StatsSave("ATK", int.Parse(ATKPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");
        stats = new StatsSave("Stamina", int.Parse(StaminaPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");
        stats = new StatsSave("Level", int.Parse(LevelPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");
        stats = new StatsSave("Skill", int.Parse(SkillPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");
        stats = new StatsSave("RequiredEXP", int.Parse(EXPPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.Write(statsData);
        SW.WriteLine(",");

        //save spawn point
        SpawnPointSave SP = new SpawnPointSave(SceneManager.GetActiveScene().buildIndex, PlayerLocation);
        JsonData SPData = JsonMapper.ToJson(SP);
        SW.Write(SPData);

        //save boss progress
    }
}

class BossSave
{
    public string name;
    public string slug = "Boss";
    public bool conquered;

    public BossSave(string name,bool conquered)
    {
        this.name = name;
        this.conquered = conquered;
    }
}

class SpawnPointSave
{
    public string name = "Scene";
    public string slug = "SpawnPoint";
    public int Scene;
    public string x;
    public string y;
    public string z;

    public SpawnPointSave(int scene, Transform SPLocation)
    {
        this.Scene = scene;
        x = SPLocation.position.x + "";
        y = SPLocation.position.y + "";
        z = SPLocation.position.z + "";
    }
}

class StatsSave
{
    public string name;
    public string slug = "Stats";
    public int Level;

    public StatsSave(string name,int level)
    {
        this.name = name;
        this.Level = level;
    }
}

class PotionSave
{
    public string name = "Potion";
    public string slug = "Potion";
    public int max_drink;

    public PotionSave(int max)
    {
        max_drink = max;
    }
}

class ItemSave
{
    public int id;
    public string name;
    public string slug = "Weapon";

    public ItemSave(Item item)
    {
        this.id = item.id;
        this.name = item.itemname;
    }
}
