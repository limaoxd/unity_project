using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class SaveGame : MonoBehaviour
{
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
    }

    public void Save()
    {
        SaveItems();
        SaveStats();
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
            SW.WriteLine(ItemData.ToString());
        }

        //save max_potion
        PotionSave potion = new PotionSave(Player.drinkMax);
        JsonData potionData = JsonMapper.ToJson(potion);
        SW.WriteLine(potionData.ToString());
    }

    public void SaveStats()
    {
        //save level
        StatsSave stats = new StatsSave("HP", int.Parse(HPPoint.text));
        JsonData statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);
        stats = new StatsSave("ATK", int.Parse(ATKPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);
        stats = new StatsSave("Stamina", int.Parse(StaminaPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);
        stats = new StatsSave("Level", int.Parse(LevelPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);
        stats = new StatsSave("Skill", int.Parse(SkillPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);
        stats = new StatsSave("RequiredEXP", int.Parse(EXPPoint.text));
        statsData = JsonMapper.ToJson(stats);
        SW.WriteLine(statsData);

        //save spawn point

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
    public string name = "SpawnPoint";
    public string slug = "SpawnPoint";
    public int scene;
    public int fire;

    public SpawnPointSave(int scene,int fire)
    {
        this.scene = scene;
        this.fire = fire;
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
    public int id = 16;
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
